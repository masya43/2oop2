using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2oop2
{
    class Motors
    {
        protected DataQwery dataQwery = new DataQwery();
        protected string name;
        protected int ls;
        private int id;

        // Постоянные:
        public virtual void StartMotor()
        {
            Console.WriteLine("Мотор " + name + " Запущен");
        }

        public virtual void StopMotor()
        {
            Console.WriteLine("Мотор " + name + " Остановлен");
        }

        // Изменяемые:
        public virtual void Remont()
        {
            Console.WriteLine("Мотор " + name + " - Обслуживание");
        }

        public virtual void AddEnergy()
        {
            Console.WriteLine("Мотор " + name + " - Заправка/Зарядка");
        }

        // Данные:
        public virtual void SetMotor(string name, int ls, int id)
        {
            this.name = name;
            this.ls = ls;
            this.id = id;
        }
        public virtual void GetMotor()
        {
            Console.WriteLine("Имя: " + name);
            Console.WriteLine("Л.С: " + ls);
        }

        public string GetName()
        {
            return name;
        }

        public int GetLs()
        {
            return ls;
        }
        public int GetId()
        {
            return id;
        }
    }

    class Electro : Motors
    {
        private int kVt;
        private string type;

        // Свои:
        public void MalieOborot()
        {
            Console.WriteLine("Мотор " + name + " - Режим малых оборотов");
        }

        public void ObratnaZaryad()
        {
            Console.WriteLine("Мотор " + name + " - Режим обратной зарядки");
        }

        // Изменяемые:
        public override void Remont()
        {
            Console.WriteLine("Мотор " + name + " - Обслуживание / Замена катушек");
        }

        public override void AddEnergy()
        {
            Console.WriteLine("Мотор " + name + " - Зарядка");
        }

        // Данные:
        public void SetMotor(int kVt, string type)
        {
            this.kVt = kVt;
            this.type = type;
        }

        public override void GetMotor()
        {
            base.GetMotor();
            Console.WriteLine("кВт: " + kVt);
        }
    }

    class DVS : Motors
    {
        private int rashod;
        private string type;

        // Свои:
        public void VidelGazov()
        {
            Console.WriteLine("Мотор " + name + " - Выделение газов");
        }

        public void VidelShyma()
        {
            Console.WriteLine("Мотор " + name + " - Выделение шума");
        }

        // Изменяемые:
        public override void Remont()
        {
            Console.WriteLine("Мотор " + name + " - Обслуживание / Замена поршневой");
        }

        public override void AddEnergy()
        {
            Console.WriteLine("Мотор " + name + " - Заправка топливом");
        }

        // Данные:
        public void SetMotor(int rashod, string type)
        {
            this.rashod = rashod;
            this.type = type;
        }

        public override void GetMotor()
        {
            base.GetMotor();
            Console.WriteLine("Расход: " + rashod);
        }
    }
}
