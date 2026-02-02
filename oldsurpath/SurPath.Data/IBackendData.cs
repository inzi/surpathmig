using MySql.Data.MySqlClient;

namespace SurPath.Data
{
    public interface IBackendDataParamObject
    {
        //bool SetParameters(MySqlCommand _cmd);
        MySqlParameter[] Parameters();

        string sp();
    }

    public interface IBackendData
    {
        //bool QueueSMS(int ClientID, int DonorId, string ApiKey, string ToPhoneNumber, string Message);
        bool QueueSMS(ParamQueueSMS paramQueueSMS);
    }
}