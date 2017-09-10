Example project in Xamarin.Android showing problem with commiting fragments after OnSaveInstanceState method is called. If we do this we get an exception "Java.Lang.IllegalStateException: Can not perform this action after onSaveInstanceState".

Running project in "Debug" configuration shows the problem.

Running in "DebugCorrect" configuration shows the solution to the problem. We can save fragments in queue and commit them after returning to the app.
