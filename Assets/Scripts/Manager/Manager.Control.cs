using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Manager : MonoBehaviour {
	private Dictionary<Swipe, Vector2> cardinalDirections = new Dictionary<Swipe, Vector2>() {
		{ Swipe.Up,         CardinalDirection.Up                 },
		{ Swipe.Down,         CardinalDirection.Down             },
		{ Swipe.Right,         CardinalDirection.Right             },
		{ Swipe.Left,         CardinalDirection.Left             },
		{ Swipe.UpRight,     CardinalDirection.UpRight             },
		{ Swipe.UpLeft,     CardinalDirection.UpLeft             },
		{ Swipe.DownRight,     CardinalDirection.DownRight         },
		{ Swipe.DownLeft,     CardinalDirection.DownLeft         }
	};

	private enum InputAction {
		click,
		swipeLeft,
		swipeRight,
		swipeUp,
		swipeDown,
		notSure
	}

	private float minSwipeLength = 0.5f;
	private float swipeCm;
	private bool useEightDirections = true;

	private const float eightDirAngle = 0.906f;
	private const float fourDirAngle = 0.5f;
	private const float defaultDPI = 72f;
	private const float dpcmFactor = 2.54f;

	private Vector2 firstPressPos = new Vector2();
	private Vector2 secondPressPos = new Vector2();


	private RaycastHit hit;
	private bool gotCast;
	private bool hitSomething;

	private bool doneSwipe;

	private int levelLayer = 17;
	private int layerPlanetMask = 13;
	private int planetLayer = 16;
	private int sunLayer = 18;
	private int layerMaskSphereCollider = 11;

	private Vector2 swipeVelocity;

	private InputAction action;

	private Swipe swipeDirection;

	private float dpcm;
	private float swipeStartTime;
	private float swipeEndTime;


	private void InitializeInput() { // get the screen dimentions
		allowInput = true;
		float dpi = (Screen.dpi == 0) ? defaultDPI : Screen.dpi;
		dpcm = dpi / dpcmFactor;
	}

	public void GetInput() {

		if (Input.GetMouseButtonDown(0)) {
			firstPressPos = (Vector2)Input.mousePosition;
			swipeStartTime = Time.time;
		}
		if (Input.GetMouseButtonUp(0)) {
			secondPressPos = (Vector2)Input.mousePosition;
			action = DetermineSwipeDirection(Camera.main.ScreenPointToRay(Input.mousePosition), firstPressPos, secondPressPos);
			doneSwipe = true;
		}
	}

	private void DoInBattle(InputAction action) {
		switch (action) { // do shit based on input
			case InputAction.click:
			break;
			case InputAction.swipeLeft:
			break;
			case InputAction.swipeRight:
			break;
			case InputAction.swipeDown:
			break;
			case InputAction.swipeUp:
			break;
			default:
			break;
		}
		ResetClickEnd();
	}

	private void DoInOrbit(InputAction action) {
		switch (action) { // do shit based on input
			case InputAction.click:
			// if (Physics.Raycast(ray, out hit, 2000, layerMaskSphereCollider)) {
			//     if (hit.collider.gameObject.layer == levelLayer) {
			//         //if (hit.transform.GetComponent<LevelMarkerScript>().AmIClickable()) {// check if clicable
			//             //SelectLevel(hit.collider.transform.parent.gameObject); // get the parent of the colider's parent, which is the level.
			//         //}
			//     } else if (hit.collider.gameObject.layer == planetLayer) {
			//         //SelectPlanet(hit.collider.gameObject);
			//     }
			// }
			break;
			case InputAction.swipeLeft:
			break;
			case InputAction.swipeRight:
			break;
			case InputAction.swipeDown:
			break;
			case InputAction.swipeUp:
			break;
			default:
			break;
		}
		ResetClickEnd();
	}
	public void MouseInput() {
		if (allowInput) {
			switch (CurrentGameState) {
				case GameState.InBattle: //when in battle
				if (!doneSwipe) {
					GetInput();
				} else {
					DoInBattle(action);
				}
				break;
				case GameState.InMenu:
				if (Input.GetMouseButton(0)) {
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
					if (Physics.Raycast(ray, out hit, 2000)) {
						if (hit.collider.gameObject.layer == planetLayer) {
							if (planetsDictionary.TryGetValue(hit.transform, out PlanetWrap planet)) {
								MoveToSelectedPlanet(planet);
							}
						}
					}
				}
				break;
				case GameState.InOrbit: // when in orbit
				if (Input.GetMouseButton(0)) {
					RotateObject(currentSelectedPlanet.planetGameObject.transform, -Input.GetAxis("Mouse X") * 10f, Input.GetAxis("Mouse Y") * 10f, Space.World);
				}
				if (Input.GetMouseButtonUp(0)) {
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
					if (Physics.Raycast(ray, out hit, 2000)) {
						if (hit.collider.gameObject.layer == levelLayer) {
							if (currentSelectedPlanet.levels.TryGetValue(hit.transform, out LevelWrap level)) {
								MoveToSelectedLevel(level);
							}
						}
					}
				}
				// if (Input.GetMouseButton(0)) {
				//     if (!gotCast) { // if I don't have a cast yet, get a cast to determine if you hit anything
				//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);// cast a ray from the camera through the touch
				//         GetFirstCast(ray);
				//     }else{
				//         if (hitSomething) {
				//             MoveMainBattle(hit.transform);
				//             if (hit.collider.gameObject.layer == planetLayer) {
				//                 RotateObject(hit.transform, -Input.GetAxis("Mouse X") * 10f, Input.GetAxis("Mouse Y") * 10f, Space.World);
				//             }

				//         } else {
				//         RotateObject(playerTransform, Input.GetAxis("Mouse X") * 10f, -Input.GetAxis("Mouse Y") * 10f, Space.World);
				//         }
				//     }
				// }
				// if (!doneSwipe){
				//     GetInput();
				// } else {
				//     DoInOrbit(action);
				// }
				break;
				default:
				break;
			}
		}

	}
	#region New Direction Detection
	void GetFirstCast(Ray ray) {
		if (Physics.Raycast(ray, out hit, 2000, layerMaskSphereCollider)) {
			Debug.DrawLine(ray.origin, hit.point);// draws the ray line
			hitSomething = true;
			SetDisplayText("hit something " + hit);
		} else {
			SetDisplayText("hit nothing " + hit);
		}
		gotCast = true;
	}

	void ResetClickEnd() {
		hitSomething = false;
		gotCast = false;
		doneSwipe = false;
	}

	private InputAction DetermineSwipeDirection(Ray ray, Vector2 firstPressPos, Vector2 secondPressPos) {
		Vector2 currentSwipe = secondPressPos - firstPressPos; // vector between first and second 
		swipeCm = currentSwipe.magnitude / dpcm;
		if (swipeCm < minSwipeLength) {
			SetDisplayText("Click");
			print("this is a click " + swipeCm);
			return InputAction.click;
		} else {
			print("this is a swipe at the end " + swipeCm);
			swipeEndTime = Time.time;
			swipeVelocity = currentSwipe * (swipeEndTime - swipeStartTime);
			swipeDirection = GetSwipeDirByTouch(currentSwipe);
			if (swipeDirection == Swipe.Left) {
				SetDisplayText("Left");
				return InputAction.swipeLeft;
			} else if (swipeDirection == Swipe.Right) {
				SetDisplayText("Right");
				return InputAction.swipeRight;
			} else if (swipeDirection == Swipe.Up) {
				SetDisplayText("Up");
				return InputAction.swipeUp;
			} else if (swipeDirection == Swipe.Down) {
				SetDisplayText("Down");
				return InputAction.swipeUp;
			} else {
				SetDisplayText("Canceled"); // this is when it's not a click but the swipe is within the distance
				return InputAction.notSure;
			}
		}
	}
	private bool IsDirection(Vector2 direction, Vector2 cardinalDirection) {
		var angle = useEightDirections ? eightDirAngle : fourDirAngle;
		return Vector2.Dot(direction, cardinalDirection) > angle;
	}

	private Swipe GetSwipeDirByTouch(Vector2 currentSwipe) {
		currentSwipe.Normalize();
		var swipeDir = cardinalDirections.FirstOrDefault(dir => IsDirection(currentSwipe, dir.Value));
		return swipeDir.Key;
	}
	#endregion
}

class CardinalDirection {
	public static readonly Vector2 Up = new Vector2(0, 1);
	public static readonly Vector2 Down = new Vector2(0, -1);
	public static readonly Vector2 Right = new Vector2(1, 0);
	public static readonly Vector2 Left = new Vector2(-1, 0);
	public static readonly Vector2 UpRight = new Vector2(1, 1);
	public static readonly Vector2 UpLeft = new Vector2(-1, 1);
	public static readonly Vector2 DownRight = new Vector2(1, -1);
	public static readonly Vector2 DownLeft = new Vector2(-1, -1);
}
// https://forum.unity.com/threads/swipe-in-all-directions-touch-and-mouse.165416/page-2#post-2741253
public enum Swipe {
	None,
	Up,
	Down,
	Left,
	Right,
	UpLeft,
	UpRight,
	DownLeft,
	DownRight
};
