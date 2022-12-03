using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VOR.Core.Model;

public enum Filtre
{
    Rien = 0,
    Boolean = 1,
}

namespace VOR.Front.Web.Commun
{
    public class MyCustomFilteringColumn : GridTemplateColumn
    {
        private Filtre _filtre = Filtre.Rien;

        public Filtre Filtre
        {
            get { return _filtre; }
            set { _filtre = value; }
        }

        protected override void SetupFilterControls(TableCell cell)
        {
            RadComboBox rcBox = new RadComboBox();

            rcBox.ID = this.UniqueName;
            rcBox.AutoPostBack = true;
            rcBox.SelectedIndexChanged += rcBox_SelectedIndexChanged;
            switch (Filtre)
            {
                case Filtre.Boolean:
                    rcBox.Width = Unit.Pixel(50);
                    rcBox.Items.Add(new RadComboBoxItem("Tous", ""));
                    rcBox.Items.Add(new RadComboBoxItem("Oui", "true"));
                    rcBox.Items.Add(new RadComboBoxItem("Non", "false"));
                    cell.Controls.Add(rcBox);
                    break;
            }
        }

        protected override void SetCurrentFilterValueToControl(TableCell cell)
        {
            if (!(this.CurrentFilterValue == ""))
            {
                ((RadComboBox)cell.Controls[0]).Items.FindItemByValue(this.CurrentFilterValue).Selected = true;
            }
        }

        protected override string GetCurrentFilterValueFromControl(TableCell cell)
        {
            string currentValue = "";
            currentValue = ((RadComboBox)cell.Controls[0]).SelectedItem.Value;
            this.CurrentFilterFunction = (currentValue != "") ? GridKnownFunction.EqualTo : GridKnownFunction.NoFilter;
            return currentValue;
        }

        private void rcBox_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ((GridFilteringItem)(((RadComboBox)sender).Parent.Parent)).FireCommandEvent("Filter", new Pair("Contains", ((RadComboBox)sender).ID));
        }

        protected void rcDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            ((GridFilteringItem)(((RadDatePicker)sender).Parent.Parent)).FireCommandEvent("Filter", new Pair());
        }
    }
}