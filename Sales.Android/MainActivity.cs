namespace Sales.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Android.Runtime;
    using ImageCircle.Forms.Plugin.Droid;

    [Activity(
        Label = "Sales",
        Icon = "@mipmap/ic_launcher",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges =
            ConfigChanges.ScreenSize |
            ConfigChanges.Orientation |
            ConfigChanges.UiMode |
            ConfigChanges.ScreenLayout |
            ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        [System.Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            ImageCircleRenderer.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(
            int requestCode,
            string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(
                requestCode,
                permissions,
                grantResults);

            base.OnRequestPermissionsResult(
                requestCode,
                permissions,
                grantResults);
        }
    }
}