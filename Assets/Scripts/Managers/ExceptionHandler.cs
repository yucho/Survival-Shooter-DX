using UnityEngine;
using System.Collections;

public class ExceptionHandler : MonoBehaviour
{
	static bool isExceptionHandlingSetup;

	/**
	 *  If anything, call this from Game Controller.
	 */
	public static void SetupExceptionHandling ()
	{
		if (!isExceptionHandlingSetup)
		{
			isExceptionHandlingSetup = true;
			Application.logMessageReceived += HandleException;
		}
	}
	
	static void HandleException (string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			Debug.LogError(condition + "\n" + stackTrace);

			/**
			 *  Pause the editor to make errors super annoying.
			 */
			Debug.Break();
		}
	}
}
