using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurPath.Data
{
    public class ParamHelper
    {
        private MySqlParameter _lastMySqlParameter = new MySqlParameter();

        private List<MySqlParameter> _ParamsList = new List<MySqlParameter>();

        public void reset()
        {
            _ParamsList = new List<MySqlParameter>();
        }
        public MySqlParameter Param
        {
            set
            {
                _lastMySqlParameter = value;
                _ParamsList.Add(_lastMySqlParameter);
            }
            get
            {
                return _lastMySqlParameter;
            }

        }
        public List<MySqlParameter> ParmList
        {
            get
            {
                return _ParamsList;
            }
            set
            {
                _ParamsList = value;
            }
        }
        public MySqlParameter[] Params
        {
            get
            {
                return _ParamsList.ToArray();
            }
        }

            


    }
}
