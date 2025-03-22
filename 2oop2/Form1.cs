using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace _2oop2
{
    public partial class FormMotors : Form
    {
        DataQwery dataQwery = new DataQwery();
        Motors motor;
        public FormMotors()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tabPageProperty.Enabled = false;
            MyMotors_Load();
            comboBox1.SelectedIndex = 0;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabPageProperty.Enabled = false;
            MyMotors_Load();
        }
        private void MyMotors_Load()
        {
            listBoxMyMotors.Items.Clear();
            DataTable dataTableMotors = dataQwery.getAllMotors(comboBox1.Text);
            foreach (DataRow row in dataTableMotors.Rows)
            {
                listBoxMyMotors.Items.Add(row["Name"].ToString());
            }
        }

        private void buttonAddNewMotor_Click(object sender, EventArgs e)
        {
            tabControlMotor.SelectedTab = tabPageAddMotors;
            radioButtonNot_CheckedChanged(sender, e);
        }
        private void buttonProperty_Click(object sender, EventArgs e)
        {
            tabPageProperty.Enabled = true;
            tabControlMotor.SelectedTab = tabPageProperty;

        }
        private void listBoxMyMotors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMyMotors.SelectedIndex != -1)
            {
                string nameMotor = listBoxMyMotors.SelectedItem.ToString();
                DataTable dataTablePropertyMotor = dataQwery.getPropertyMotor(nameMotor);
                dataGridViewMotor.DataSource = dataTablePropertyMotor;
                dataGridViewMotor.Columns[1].Width = 180;
                motorProperty(nameMotor);
            }
        }

        private void radioButtonNot_CheckedChanged(object sender, EventArgs e)
        {
            textBoxParametr1.Visible = false;
            comboBoxParametr2.Visible = false;
            labelParametr1.Visible = false;
            labelParametr2.Visible = false;
        }
        private void radioButtonDVS_CheckedChanged(object sender, EventArgs e)
        {
            textBoxParametr1.Visible = true;
            textBoxParametr1.Text = "";
            labelParametr1.Visible = true;
            labelParametr1.Text = "Расход:";
            labelParametr2.Visible = true;
            labelParametr2.Text = "Тип: Дизельный/Бензиновый";
            comboBoxParametr2.Visible = true;
            comboBoxParametr2.Text = "";
            comboBoxParametr2.Items.Clear();
            comboBoxParametr2.Items.Add("Дизельный");
            comboBoxParametr2.Items.Add("Бензиновый");

        }

        private void radioButtonElectro_CheckedChanged(object sender, EventArgs e)
        {
            textBoxParametr1.Visible = true;
            textBoxParametr1.Text = "";
            labelParametr1.Visible = true;
            labelParametr1.Text = "кВт:";
            labelParametr2.Visible = true;
            labelParametr2.Text = "Тип: Трехфазный/Двухфазный";
            comboBoxParametr2.Visible = true;
            comboBoxParametr2.Text = "";
            comboBoxParametr2.Items.Clear();
            comboBoxParametr2.Items.Add("Трехфазный");
            comboBoxParametr2.Items.Add("Двухфазный");
        }
        private void buttonAddMotor_Click(object sender, EventArgs e)
        {
            if (!listBoxMyMotors.Items.Contains(textBoxName.Text))
            {
                Motors newMotor;
                int id = dataQwery.insertNewMotor();
                if (radioButtonDVS.Checked)
                {
                    newMotor = new DVS();
                    int rashod = int.Parse(textBoxParametr1.Text);
                    string type = comboBoxParametr2.Text;
                    ((DVS)newMotor).SetMotor(rashod, type);
                    dataQwery.insertNewDVS(rashod, type, id);
                }
                else if (radioButtonElectro.Checked)
                {
                    newMotor = new Electro();
                    int kVt = int.Parse(textBoxParametr1.Text);
                    string type = comboBoxParametr2.Text;
                    ((Electro)newMotor).SetMotor(kVt, type);
                    dataQwery.insertNewElectro(kVt, type, id);
                }
                else
                {
                    newMotor = new Motors();
                }
                newMotor.SetMotor(textBoxName.Text, int.Parse(textBoxLS.Text), id);
                dataQwery.updateNewMotor(newMotor.GetName(), newMotor.GetLs(), id);
                clearFromAdd();
                MyMotors_Load();
            }
            else MessageBox.Show("Имя занято");
        }
        private void clearFromAdd()
        {
            textBoxName.Clear();
            textBoxLS.Clear();
            textBoxParametr1.Clear();
            comboBoxParametr2.Text = "";
            tabPageProperty.Enabled = false;
        }
        private void motorProperty(string nameMotor)
        {
            labelUpdParametr1.Visible = true;
            labelUpdParametr2.Visible = true;
            textBoxUpdParametr1.Visible = true;
            comboBoxUpdParametr2.Visible = true;
            buttonParametr1.Visible = true;
            buttonParametr2.Visible = true;
            DataTable dataTableMotor = dataQwery.getMotor(nameMotor);
            int id = dataTableMotor.Rows[0]["id"] != DBNull.Value ? Convert.ToInt32(dataTableMotor.Rows[0]["id"]) : 0;
            int ls = dataTableMotor.Rows[0]["ls"] != DBNull.Value ? Convert.ToInt32(dataTableMotor.Rows[0]["ls"]) : 0;
            int rashod_kVt = dataTableMotor.Rows[0]["rashod_kVt"] != DBNull.Value ? Convert.ToInt32(dataTableMotor.Rows[0]["rashod_kVt"]) : 0;
            string classType = dataTableMotor.Rows[0]["class"]?.ToString() ?? "";
            string motorType = dataTableMotor.Rows[0]["type"]?.ToString() ?? "";
            switch (classType)
            {
                case "Электрический":
                    motor = new Electro();
                    ((Electro)motor).SetMotor(rashod_kVt, motorType);
                    radioButtonUpdElectro.Checked = true;
                    electroProperty();
                    break;
                case "Внутреннего сгорания":
                    motor = new DVS();
                    radioButtonUpdDVS.Checked = true;
                    ((DVS)motor).SetMotor(rashod_kVt, motorType);
                    dvsProperty();
                    break;
                default:
                    motor = new Motors();
                    radioButtonUpdNot.Checked = true;
                    labelUpdParametr1.Visible = false;
                    labelUpdParametr2.Visible = false;
                    textBoxUpdParametr1.Visible = false;
                    comboBoxUpdParametr2.Visible = false;
                    buttonParametr1.Visible = false;
                    buttonParametr2.Visible = false;
                    break;
            }
            motor.SetMotor(nameMotor, ls, id);
            textBoxUpdName.Text = motor.GetName();
            textBoxUpdLS.Text = motor.GetLs().ToString();
            textBoxUpdParametr1.Text = rashod_kVt.ToString();
            comboBoxUpdParametr2.Text = motorType;
        }
        private void electroProperty()
        {
            labelUpdParametr1.Text = "кВт";
            labelUpdParametr2.Text = "Тип";
            comboBoxUpdParametr2.Items.Clear();
            comboBoxUpdParametr2.Items.Add("Трехфазный");
            comboBoxUpdParametr2.Items.Add("Двухфазный");
            buttonParametr1.Text = "Малые обороты";
            buttonParametr2.Text = "Обратная зарядка";
        }
        private void dvsProperty()
        {
            labelUpdParametr1.Text = "Расход";
            labelUpdParametr2.Text = "Тип";
            comboBoxUpdParametr2.Items.Clear();
            comboBoxUpdParametr2.Items.Add("Дизельный");
            comboBoxUpdParametr2.Items.Add("Бензиновый");
            buttonParametr1.Text = "Выделение газов";
            buttonParametr2.Text = "Выделение шума";
        }

        private void buttonDeleteMotor_Click(object sender, EventArgs e)
        {
            if (listBoxMyMotors.SelectedIndex != -1)
            {
                dataQwery.deleteMotor(listBoxMyMotors.SelectedItem.ToString());
            }
            MyMotors_Load();
            tabPageProperty.Enabled = false;
        }

        private void tabControlMotor_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (listBoxMyMotors.SelectedIndex != -1)
            {
                MyMotors_Load();
                labelMotor.Text = motor.GetName();
                tabPageProperty.Enabled = true;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            labelСondition.Text = "Заводим двигатель";
            motor.StartMotor();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            labelСondition.Text = "Глушим двигатель";
            motor.StopMotor();
        }

        private void buttonRemont_Click(object sender, EventArgs e)
        {
            labelСondition.Text = "Обслуживание";
            motor.Remont();
        }

        private void buttonAddEnergy_Click(object sender, EventArgs e)
        {
            labelСondition.Text = "Заправка/Зарядка";
            motor.AddEnergy();
        }

        private void buttonParametr1_Click(object sender, EventArgs e)
        {
            if (radioButtonUpdDVS.Checked)
            {
                ((DVS)motor).VidelGazov();
                labelСondition.Text = "Выделение газов";
            }
            else if (radioButtonUpdElectro.Checked)
            {
                ((Electro)motor).MalieOborot();
                labelСondition.Text = "Малые обороты";
            }
        }

        private void buttonParametr2_Click(object sender, EventArgs e)
        {
            if (radioButtonUpdDVS.Checked)
            {
                ((DVS)motor).VidelShyma();
                labelСondition.Text = "Выделение шума";
            }
            else if (radioButtonUpdElectro.Checked)
            {
                ((Electro)motor).ObratnaZaryad();
                labelСondition.Text = "Обратная зарядка";
            }
        }
        private void buttonUpdSave_Click(object sender, EventArgs e)
        {
            dataQwery.updateMotor(textBoxUpdName.Text, int.Parse(textBoxUpdLS.Text), motor.GetId());
            if (radioButtonUpdDVS.Checked)
            {
                dataQwery.updateDVS(int.Parse(textBoxUpdParametr1.Text), comboBoxUpdParametr2.Text, motor.GetId());
            }
            else if (radioButtonUpdElectro.Checked)
            {
                dataQwery.updateElectro(int.Parse(textBoxUpdParametr1.Text), comboBoxUpdParametr2.Text, motor.GetId());
            }
            clearFromUpd();
            MyMotors_Load();
        }
        private void clearFromUpd()
        {
            textBoxUpdName.Clear();
            textBoxUpdLS.Clear();
            textBoxUpdParametr1.Clear();
            comboBoxUpdParametr2.Text = "";
            tabPageProperty.Enabled = false;
            labelСondition.Text = "";
        }
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная программа создана в ученических целях, разработал студент 2 курса группы ИВТб-2305 Медведев М.Е.");
        }

        private void tabControlMotor_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

    }
}
