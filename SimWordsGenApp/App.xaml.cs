using Prism.Ioc;
using Prism.Unity;
using SimWordsGenApp.Views;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace SimWordsGenApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            string currentType = "";
            try
            {
                //containerRegistry.RegisterInstance(Container.Resolve<PrismFactoryService>((typeof(IContainerProvider), Container)));

                var types = Assembly.GetExecutingAssembly().GetTypes();

                foreach (var type in types.Where(t => t.IsClass && !t.IsAbstract && t.CustomAttributes.Any(a => a.AttributeType == typeof(PrismSingletonAttribute))))
                    containerRegistry.RegisterSingleton(type, type);

                foreach (var type in types.Where(t => t.IsClass && !t.IsAbstract && t.CustomAttributes.Any(a => a.AttributeType == typeof(PrismResourceInjectionAttribute))))
                {
                    var attribute = Attribute.GetCustomAttribute(type, typeof(PrismResourceInjectionAttribute)) as PrismResourceInjectionAttribute;
                    currentType = attribute.ResourceKey ?? type.Name;
                    //Logger.Trace($"Trying to resolve '{currentType}'");
                    Resources.Add(currentType, Container.Resolve(type));
                }

                foreach (var type in types.Where(t => t.IsClass && !t.IsAbstract && t.CustomAttributes.Any(a => a.AttributeType == typeof(PrismGenericResourceInjectionAttribute))))
                {
                    var attribute = Attribute.GetCustomAttribute(type, typeof(PrismGenericResourceInjectionAttribute)) as PrismGenericResourceInjectionAttribute;
                    var resultType = attribute.GenericType.IsGenericTypeDefinition ? attribute.GenericType.MakeGenericType(type) : attribute.GenericType;
                    var key = attribute.ResourceKey;
                    if (key == null)
                        if (attribute.GenericType.CustomAttributes.Any(a => a.AttributeType == typeof(PrismResourceKeyFormatAttribute)))
                        {
                            var gAttribute = Attribute.GetCustomAttribute(attribute.GenericType, typeof(PrismResourceKeyFormatAttribute)) as PrismResourceKeyFormatAttribute;
                            key = string.Format(gAttribute.KeyFormat, type.Name);
                        }
                        else
                            key = $"{attribute.GenericType.Name}{type.Name}";
                    currentType = resultType.ToString(); ;
                    //Logger.Trace($"Trying to resolve '{currentType}'");
                    Resources.Add(key, Container.Resolve(resultType));
                }

                //var sss = Container.Resolve<PrismFactoryService>();
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, $"RegisterType: '{currentType}'");
                MessageBox.Show(ex.Message);
            }
        }

        protected override Window CreateShell()
        {
            //Logger.Trace($"CreateShell");
            try
            {
                var w = Container.Resolve<MainWindow>();
                return w;
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, "CreateShell");
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void ForTests()
        {
            var t = new ViewModels.MainWindowVMDummy();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ForTests();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //Logger.Trace($"OnExit");
            try
            {
                Settings.Save();

                base.OnExit(e);
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, "OnExit");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
