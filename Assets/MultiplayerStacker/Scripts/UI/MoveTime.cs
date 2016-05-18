using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Responsible for updating the current players move time.
	/// </summary>
	[RequireComponent (typeof(GameText), typeof(Animator), typeof(RectTransform))]
	public class MoveTime : MonoBehaviour
	{
		/// <summary>
		/// The low on time threshold. When the players current time is below this threshold the time will pulse and move to the centre of the screen,
		/// </summary>
		public int lowOnTimeThreshold;

		/// <summary>
		/// The colour of the countdown text when the time is above the <see cref="MultiStack.MoveTime.lowOnTimeThreshold"/>.
		/// </summary>
		public Color normalTimeTextColour;

		/// <summary>
		/// The colour of the countdown text when the time is below the <see cref="MultiStack.MoveTime.lowOnTimeThreshold"/>.
		/// </summary>
		public Color lowTimeTextColour;

		private static int LOW_ON_TIME_HASH = Animator.StringToHash ("lowOnTime");
		private static readonly Vector2 CENTRED_VECTOR = new Vector2 (0.5f, 0.5f);
		private static readonly Vector2 ZEROED_VECTOR = Vector2.zero;
		private static readonly Vector2 TOP_LEFT_VECTOR = new Vector2 (0, 1f);
		private static readonly Vector2 TOP_LEFT_LOCATION = new Vector2 (66.2f, -24.6f);

		private GameText _gameText;
		private Animator _animatior;
		private RectTransform _rectTransform;
		private bool _moved;
		private int previousPlayer;

		void Start ()
		{
			_gameText = GetComponent<GameText> ();
			_animatior = GetComponent<Animator> ();
			_rectTransform = GetComponent<RectTransform> ();

			_gameText.ChangeFontColour (normalTimeTextColour);
		}

		/// <summary>
		/// Updates time animation and position based on time remaining.
		/// </summary>
		void Update ()
		{
			if (previousPlayer != TurnManager.instance.currentPlayer) {
				previousPlayer = TurnManager.instance.currentPlayer;
				_gameText.HideText ();
			}

			int currentTime;

			if (int.TryParse (_gameText.text, out currentTime)) {
				if (currentTime <= lowOnTimeThreshold) {
					_animatior.SetBool (LOW_ON_TIME_HASH, true);
					_gameText.ChangeFontColour (lowTimeTextColour);

					TranslateRectTransform (CENTRED_VECTOR, CENTRED_VECTOR, ZEROED_VECTOR);
				} else {
					_animatior.SetBool (LOW_ON_TIME_HASH, false);
					_gameText.ChangeFontColour (normalTimeTextColour);

					TranslateRectTransform (TOP_LEFT_VECTOR, TOP_LEFT_VECTOR, TOP_LEFT_LOCATION);
				}
			}
	
		}

		private void TranslateRectTransform (Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition)
		{
			_rectTransform.anchorMin = anchorMin;
			_rectTransform.anchorMax = anchorMax;
			_rectTransform.anchoredPosition = anchoredPosition;
		}
	}
}
