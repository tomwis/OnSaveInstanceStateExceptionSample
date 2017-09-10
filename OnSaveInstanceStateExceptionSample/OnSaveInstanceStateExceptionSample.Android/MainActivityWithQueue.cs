#if DEBUG_CORRECT
using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace OnSaveInstanceStateExceptionSample.Droid
{
	[Activity (Label = "OnSaveInstanceStateExceptionSample.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivityWithQueue : Activity
	{
        Switch _switch;
        TextView _instruction;
        bool _instanceStateSaved;
        List<Type> _fragmentsQueue;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            _fragmentsQueue = new List<Type>();

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            _switch = FindViewById<Switch>(Resource.Id.switch1);
            _switch.CheckedChange += Switch1_CheckedChange;

            _instruction = FindViewById<TextView>(Resource.Id.instruction);
            _instruction.Text = "Now leave the app with home button (not back button) and open it again. It won't crash and fragment will load.";
        }

        private void Switch1_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            _instruction.Visibility = e.IsChecked ? ViewStates.Visible : ViewStates.Gone;
        }

        void AddFragment(Type fragmentType)
        {
            System.Diagnostics.Debug.WriteLine("AddFragment");
            var fragment = (Fragment) Activator.CreateInstance(fragmentType);
            FragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.fragment_container, fragment)
                .Commit();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            _instanceStateSaved = true;
            base.OnSaveInstanceState(outState);
            System.Diagnostics.Debug.WriteLine("OnSaveInstanceState");
        }

        protected override void OnPostResume()
        {
            base.OnPostResume();
            System.Diagnostics.Debug.WriteLine("OnPostResume");
            _instanceStateSaved = false;

            for(int i = 0; i < _fragmentsQueue.Count;)
            {
                AddFragment(_fragmentsQueue[i]);
                _fragmentsQueue.RemoveAt(0);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            System.Diagnostics.Debug.WriteLine("OnPause");
        }

        protected override void OnStop()
        {
            base.OnStop();
            System.Diagnostics.Debug.WriteLine("OnStop");
            
            if (_switch.Checked)
            {
                if (_instanceStateSaved)
                {
                    System.Diagnostics.Debug.WriteLine("Save fragment for later");
                    _fragmentsQueue.Add(typeof(MyFragment));
                }
                else
                {
                    AddFragment(typeof(MyFragment));
                }
            }
        }
    }
}
#endif
