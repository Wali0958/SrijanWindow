using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SrijanAutoStartCount
{
    class DataUtility
    {
        #region AllProperties
        private SqlConnection _con;
        public SqlConnection con
        {
            get { return _con; }
            set { _con = value; }
        }
        private SqlCommand _cmd;
        public SqlCommand cmd
        {
            get { return _cmd; }
            set { _cmd = value; }
        }
        private SqlDataReader _dr;
        public SqlDataReader dr
        {
            get { return _dr; }
            set { _dr = value; }
        }
        private SqlDataAdapter _sda;
        public SqlDataAdapter sda
        {
            get { return _sda; }
            set { _sda = value; }
        }
        private DataTable _dt;
        public DataTable dt
        {
            get { return _dt; }
            set { _dt = value; }
        }
        private DataSet _ds;
        public DataSet ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        #endregion
        #region All PrivateMethod(s)
        /// <summary>
        /// This is a Function to Open Connection
        /// </summary>
        [Obsolete]
        private void OpenConnection()
        {
            try
            {
                if (_con == null)
                {
                    _con = new SqlConnection((ConfigurationSettings.AppSettings["ConnectionString"]));
                }
                if (_con.State == ConnectionState.Closed)
                {
                    _con.Open();
                }
                _cmd = new SqlCommand();
                _cmd.Connection = _con;
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// This is a Function to Close the Connection
        /// </summary>
        private void CloseConnection()
        {
            if (_con.State == ConnectionState.Open)
            {
                _con.Close();
            }
        }
        /// <summary>
        /// This is a Function to Dispose the Connection.
        /// </summary>
        private void DisposeConnection()
        {
            if (_con != null)
            {
                _con.Dispose();
                _con = null;
            }

        }
        #endregion
        #region All PublicMethod(s)
        /// <summary>
        /// This is a Function to Execute DML
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        [Obsolete]
        public int ExecuteSql(string strsql)
        {
            OpenConnection();
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = strsql;
            _cmd.CommandTimeout = 30;
            int result = _cmd.ExecuteNonQuery();
            CloseConnection();
            return result;
        }
        /// <summary>
        /// This is a Function to Execute DML using StoredProcedure.
        /// </summary>
        /// <param name="SPName"></param>
        /// <param name="arrparam"></param>
        /// <returns></returns>
        public int ExecuteSql(String SPName, SqlParameter[] arrparam)
        {
            OpenConnection();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = SPName;
            if (_cmd.Parameters.Count > 0)
            {
                _cmd.Parameters.Clear();
            }
            if (arrparam != null)
            {
                foreach (SqlParameter param in arrparam)
                {
                    _cmd.Parameters.Add(param);
                }
            }
            int result = _cmd.ExecuteNonQuery();
            CloseConnection();
            DisposeConnection();
            return result;
        }
        /// <summary>
        /// This is to Validate a record if it is already there in the table.
        /// </summary>
        /// <param name="StrSql"></param>
        /// <returns></returns>
        [Obsolete]
        public bool IsExit(String StrSql)
        {
            OpenConnection();
            //
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = StrSql;
            cmd.CommandTimeout = 30;
            dr = cmd.ExecuteReader();
            if (dr.Read())
                return true;
            else
                return false;
        }
        /// <summary>
        /// This is a Function to GetData Using SqlDatareder
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        [Obsolete]
        public SqlDataReader GetData(string strSql)
        {
            OpenConnection();
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandTimeout = 0;
            _cmd.CommandText = strSql;
            dr = _cmd.ExecuteReader();
            return dr;
        }
        /// <summary>
        /// This is a function to GetData Using Stored Procedure
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [Obsolete]
        public SqlDataReader GetData(string strProc, SqlParameter param)
        {
            OpenConnection();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.Parameters.Add(param);
            _cmd.CommandTimeout = 30;
            _cmd.CommandText = strProc;
            dr = _cmd.ExecuteReader();
            return dr;
        }
        /// <summary>
        /// This is a function to GetData Using Stored Procedure
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [Obsolete]
        public SqlDataReader GetDataProc(string strProc)
        {
            OpenConnection();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandTimeout = 30;
            _cmd.CommandText = strProc;
            dr = _cmd.ExecuteReader();
            return dr;
        }
        /// <summary>
        /// This is a parameterized function to access the data to the datareader 
        /// </summary>
        /// <param name="strSPname"></param>
        /// <param name="arrparams"></param>
        /// <returns></returns>
        [Obsolete]
        public SqlDataReader Getdatareader(string strsql, SqlParameter[] arrparams)
        {
            OpenConnection();
            //dReader=new SqlDataReader();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = strsql;
            cmd.CommandTimeout = 30;
            //This is to clean the existing values lies on parameters property
            if (cmd.Parameters.Count > 0)
            {
                cmd.Parameters.Clear();
            }
            if (arrparams != null)
            {
                foreach (SqlParameter param in arrparams)
                {
                    cmd.Parameters.Add(param);
                }
            }
            dr = cmd.ExecuteReader();
            return dr;
        }
        /// <summary>
        /// This is a Function to GetDataTable using DataTable with SqlDataAdapter
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        [Obsolete]
        public DataTable GetTable(string strSql)
        {
            OpenConnection();
            SqlDataAdapter mda = new SqlDataAdapter();
            DataTable dt1 = new DataTable();
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = strSql;
            mda.SelectCommand = _cmd;
            mda.Fill(dt1);
            CloseConnection();
            DisposeConnection();
            return dt1;
        }
        /// <summary>
        /// This is a parameterized function to access the data to the datatable from dataadopter.
        /// </summary>
        /// <param name="strSPname"></param>
        /// <param name="arrparams"></param>
        /// <returns></returns>
        [Obsolete]
        public DataTable getdatatable(string strSPname, SqlParameter[] arrparams)
        {

            OpenConnection();
            //create datatable object
            DataTable dt = new DataTable();
            sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = strSPname;
            if (cmd.Parameters.Count > 0)
            {
                cmd.Parameters.Clear();
            }
            if (arrparams != null)
            {
                foreach (SqlParameter param in arrparams)
                {
                    cmd.Parameters.Add(param);
                }
            }
            sda.SelectCommand = cmd;
            sda.Fill(dt);
            CloseConnection();
            DisposeConnection();
            return dt;
        }
        [Obsolete]
        public DataSet GetDataset(string strSql, string TableName, SqlParameter[] arrparams)
        {
            DataSet dss = new DataSet();
            SqlDataAdapter mda = new SqlDataAdapter();
            OpenConnection();
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = strSql;
            if (cmd.Parameters.Count > 0)
            {
                cmd.Parameters.Clear();
            }
            if (arrparams != null)
            {
                foreach (SqlParameter param in arrparams)
                {
                    cmd.Parameters.Add(param);
                }
            }
            mda.SelectCommand = _cmd;
            mda.Fill(dss, TableName);
            CloseConnection();
            DisposeConnection();
            return dss;

        }
        [Obsolete]
        public DataSet GetDataset(string strSql, string TableName)
        {
            DataSet dss = new DataSet();
            SqlDataAdapter mda = new SqlDataAdapter();
            OpenConnection();
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = strSql;
            mda.SelectCommand = _cmd;
            mda.Fill(dss, TableName);
            CloseConnection();
            DisposeConnection();
            return dss;
        }
        #endregion
    }
}
