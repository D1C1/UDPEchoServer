using System;
using System.Collections.Generic;
using System.Text;

namespace UDPBroadFanOutServer
{
    class SensorDataModel
    {
        //"measurmentId": 0,
        //"sensorID": 0,
        //"roomID": "string",
        //"temperature": 0,
        //"humidity": 0,
        //"cO2": 0,
        //"pressure": 0,
        //"timeStamp": "2021-04-19T12:24:19.651Z"
        public SensorDataModel(string id, int temp, int measurmentId, int sensorId, int humidity, int co2, int pressure)
        {
            RoomId = id;
            Temperature = temp;
            MeasurmentId = measurmentId;
            SensorID = sensorId;
            Humidity = humidity;
            CO2 = co2;
            Pressure = pressure;
            TimeStamp = DateTime.Now;
        }

        public SensorDataModel()
        {

        }

        public int MeasurmentId { get; set; }
        public int SensorID { get; set; }
        public string RoomId { get; set; }
        public int Temperature { get; set; }
        public int Humidity { get; set; }
        public int CO2 { get; set; }
        public int Pressure { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}