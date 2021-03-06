﻿#if !DEBUG_CORRECT
using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


namespace OnSaveInstanceStateExceptionSample.Droid
{
    [Activity(Label = "OnSaveInstanceStateExceptionSample.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Switch _switch;
        TextView _instruction;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _switch = FindViewById<Switch>(Resource.Id.switch1);
            _switch.CheckedChange += Switch1_CheckedChange;

            _instruction = FindViewById<TextView>(Resource.Id.instruction);
            _instruction.Text = "Now leave the app with home button (not back button) to crash it.";
        }

        private void Switch1_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            _instruction.Visibility = e.IsChecked ? ViewStates.Visible : ViewStates.Gone;
        }

        void AddFragment()
        {
            System.Diagnostics.Debug.WriteLine("AddFragment");
            var fragment = new MyFragment();
            FragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.fragment_container, fragment)
                .Commit();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            System.Diagnostics.Debug.WriteLine("OnSaveInstanceState");
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
                AddFragment();
            }
        }
    }
}
#endif