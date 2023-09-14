using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegacyExplorer.Processors
{
    public class FormsScanner
    {
        public List<string> ScanForms(string dllPath)
        {
            List<string> infodllInfo = new List<string>();

            try
            {
                // Load the assembly from the DLL file.
                Assembly assembly = Assembly.LoadFrom(dllPath);
                IEnumerable<TypeInfo> tyInfo = assembly.DefinedTypes.ToArray();
                //Type[] tyInfo = assembly.GetTypes();
                foreach (Type t in tyInfo) {
                    
                    switch (t.BaseType.ToString())
                    {
                        case "System.Object":
                            infodllInfo.Add($"file Type:{t.BaseType}, file Name: {t.Name}");
                            break;
                        case "System.Windows.Forms.Form":
                            // Get all types defined in the assembly that inherit from Control.
                            Type[] formTypes = assembly.GetTypes().Where(type => typeof(Form).IsAssignableFrom(type)).ToArray();


                            // Display the names of control types.
                            foreach (Type formType in formTypes)
                            {

                                foreach (FieldInfo field in formType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                                {
                                    if (typeof(Control).IsAssignableFrom(field.FieldType))
                                    {
                                        // Output the control's field name.
                                        infodllInfo.Add($"{formType.Name}, {formType.FullName}, {field.Name}, {field.FieldType}");
                                    }

                                }
                            }
                            break;
                        case "System.Configuration.ApplicatoinSettingsBase":
                            infodllInfo.Add($"file Type:{t.BaseType}, file Name: {t.Name}");
                            break;
                        default:
                            infodllInfo.Add($"file Type:{t.BaseType}, file Name: {t.Name}" );
                            break;
                    }
                }

               
             
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception while scanning {dllPath}", ex);
            }

            return infodllInfo;

        }

    }

}
