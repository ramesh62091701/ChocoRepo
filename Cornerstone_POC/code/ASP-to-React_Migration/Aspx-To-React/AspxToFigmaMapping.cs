using Extractor.Model;
using Newtonsoft.Json;


namespace Aspx_To_React
{
    public partial class AspxToFigmaMapping : Form
    {
        private int index = 1;
        private Request request;
        private List<AspComponent> aspxComponents;
        private List<FigmaComponent> figmaComponents;
        public List<MappedControl> MappedControls { get; }
        public AspxToFigmaMapping()
        {
            InitializeComponent();
            MappedControls = new List<MappedControl>();
        }

        public void Initialize(Request request)
        {
            this.request = request;
            
            //filter components with empty id
            aspxComponents =  request.Components.AspComponents.FindAll(x => !String.IsNullOrEmpty(x.id));
            aspxComponents.Insert(0, new AspComponent { id = String.Empty});
            cmbAspx1.DataSource = aspxComponents;
            cmbAspx1.DisplayMember = "id";
            cmbAspx1.ValueMember = "id";
            cmbAspx1.SelectedIndex = 0;

            figmaComponents = request.Components.FigmaComponents;
            figmaComponents.Insert(0, new FigmaComponent { Type = string.Empty, Name = string.Empty });
            cmbFigma1.DataSource = figmaComponents;
            cmbFigma1.DisplayMember = "Id";
            cmbFigma1.ValueMember = "Id";
            cmbFigma1.SelectedIndex = 0;

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
                if (!string.IsNullOrEmpty(figmaControl?.SelectedValue as string) &&
                    !string.IsNullOrEmpty(aspxControl?.SelectedValue as string))
                {
                    var aspComponent = aspxComponents.First(x => x.id == aspxControl?.SelectedValue as string);
                    var figmaComponent = figmaComponents.First(x => x.Id == figmaControl?.SelectedValue as string);
                    MappedControls.Add(new MappedControl() { AspComponent = aspComponent, FigmaComponent = figmaComponent });
                }
            }
            this.Close();
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
            comboBox.BringToFront();

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
