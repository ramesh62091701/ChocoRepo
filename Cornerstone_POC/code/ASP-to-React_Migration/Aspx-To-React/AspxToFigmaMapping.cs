using Extractor.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aspx_To_React
{
    public partial class AspxToFigmaMapping : Form
    {
        private int index = 1;
        private Request request;
        private List<AspControl> aspxComponents;
        private List<FigmaComponent> figmaComponents;
        public Dictionary<string, string> Mapping { get; set; }
        public AspxToFigmaMapping()
        {
            InitializeComponent();
        }

        public void Initialize(Request request)
        {
            Mapping = new Dictionary<string, string>();
            this.request = request;
            
            //filter components with empty id
            aspxComponents =  request.ControlResponse.AspxComponents.FindAll(x => !String.IsNullOrEmpty(x.id));
            aspxComponents.Insert(0, new AspControl { id = String.Empty});
            cmbAspx1.DataSource = aspxComponents;
            cmbAspx1.DisplayMember = "id";
            cmbAspx1.ValueMember = "id";
            //cmbAspx1.SelectedIndex = 0;

            figmaComponents = request.ControlResponse.FigmaComponents.FindAll(x => !String.IsNullOrEmpty(x.TableName) || !String.IsNullOrEmpty(x.PropertyName)); ;
            figmaComponents.Insert(0, new FigmaComponent { Type = string.Empty });
            cmbFigma1.DataSource = figmaComponents;
            cmbFigma1.DisplayMember = "TableName";
            cmbFigma1.ValueMember = "TableName";
            //cmbFigma1.SelectedIndex = 0;

            for (int i = 0; i < 3; i++)
            {
                Add();
            }
           
        }

        private void btnSumit_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= index; i++)
            {
                var figmaControl = this.Controls.Find($"cmbFigma{i}", false)[0] as ComboBox;
                var aspxControl = this.Controls.Find($"cmbAspx{i}", false)[0] as ComboBox;
                if (!string.IsNullOrEmpty(figmaControl?.SelectedText) &&
                    !string.IsNullOrEmpty(aspxControl?.SelectedText))
                {
                    Mapping.Add(figmaControl.Text, aspxControl.Text);
                }
            }
        }

        private ComboBox AddComboBox(ComboBox referenceComboBox, int index, string prefix)
        {
            // Create a new ComboBox
            ComboBox comboBox = new ComboBox();

            // Set properties
            comboBox.Location = new System.Drawing.Point(referenceComboBox.Location.X, referenceComboBox.Location.Y + 40); // Set location on the form
            comboBox.Size = referenceComboBox.Size;
            comboBox.Name = $"{prefix}{index}";
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList; // Optional: set drop-down style
            comboBox.DataSource = prefix == "cmbFigma" ? Clone(figmaComponents) : Clone(aspxComponents);
            comboBox.DisplayMember = referenceComboBox.DisplayMember;
            comboBox.ValueMember = referenceComboBox.ValueMember;
            // Optionally set default selected item
            //comboBox.SelectedIndex = 0;

            // Add the ComboBox to the Form's controls
            this.Controls.Add(comboBox);
            return comboBox;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }

        private T Clone<T>(T obj)
        {
            var serializedObject = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        private void Add()
        {
            var figmaControl = this.Controls.Find($"cmbFigma{index}", false)[0] as ComboBox;
            var aspxControl = this.Controls.Find($"cmbAspx{index}", false)[0] as ComboBox;
            index++;
            AddComboBox(figmaControl, index, "cmbFigma");
            AddComboBox(aspxControl, index, "cmbAspx");
        }
    }
}
