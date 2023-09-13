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
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception while scanning {dllPath}", ex);
            }

            return infodllInfo;

        }

    }

}
