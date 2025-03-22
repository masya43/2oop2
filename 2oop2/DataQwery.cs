using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _2oop2
{
    internal class DataQwery
    {
        DataBase _dataBase = new DataBase();
        public DataQwery() { }
        public DataTable getAllMotors(string classType)
        {
            string query = $"SELECT name FROM motors ";
            query += classType == "Все" ? "": $" WHERE class LIKE '{classType}'";
            return _dataBase.requestQuery(query);
        }
        public DataTable getPropertyMotor(string name)
        {
            string query = $"SELECT * FROM dbo.GetMotorCharacteristicsByName('{name}')";
            return _dataBase.requestQuery(query);
        }
        public DataTable getMotor(string name)
        {
            string query = $"EXEC dbo.GetMotor @name = '{name}'";
            return _dataBase.requestQuery(query);
        }
        public int insertNewMotor()
        {
            string query = $"INSERT INTO dbo.Motors OUTPUT INSERTED.id DEFAULT VALUES;";
            return _dataBase.requestQueryWithOutput(query);
        }
        public void updateNewMotor(string name, int ls, int id)
        {
            string query = $"UPDATE dbo.Motors SET name = '{name}',ls = {ls} WHERE id = {id}";
            _dataBase.requestQuery(query);
        }
        public void insertNewDVS(int rashod, string type, int id)
        {
            string query = $@"INSERT INTO dbo.DVS (rashod, type, id_motors) VALUES ('{rashod}', '{type}', {id});
                                UPDATE dbo.Motors SET class = 'Внутреннего сгорания' WHERE id = {id}";
            _dataBase.requestQuery(query);
        }
        public void insertNewElectro(int kVt, string type, int id)
        {
            string query = $@"INSERT INTO dbo.Electro (kVt, type, id_motors) VALUES ('{kVt}', '{type}', {id});
                                UPDATE dbo.Motors SET class = 'Электрический' WHERE id = {id}";
            _dataBase.requestQuery(query);
        }
        public void deleteMotor(string name)
        {
            string query = $@"DELETE FROM dbo.Electro WHERE id_motors = (SELECT id FROM dbo.Motors WHERE name = '{name}');
                              DELETE FROM dbo.DVS WHERE  id_motors = (SELECT id FROM dbo.Motors WHERE name = '{name}');
                              DELETE FROM dbo.Motors WHERE name = '{name}';";
            _dataBase.requestQuery(query);
        }
        public void updateMotor(string name, int ls, int id)
        {
            string query = $"UPDATE dbo.Motors SET name = '{name}',ls = {ls} WHERE id = {id}";
            _dataBase.requestQuery(query);
        }
        public void updateDVS(int rashod, string type, int id)
        {
            string query = $"UPDATE dbo.DVS SET rashod = {rashod},type = '{type}' WHERE id_motors = {id}";
            _dataBase.requestQuery(query);
        }
        public void updateElectro(int kVt, string type, int id)
        {
            string query = $"UPDATE dbo.Electro SET kVt = {kVt},type = '{type}' WHERE id_motors = {id}";
            _dataBase.requestQuery(query);
        }
    }
}
