using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connector
{


    public class ConnectionRequestEventArgs : EventArgs
    {
        string _name;

        public ConnectionRequestEventArgs(String name)
        {
            this._name = name;
        }

        public String PlayerName
        {
            get { return _name; }
        }
    }

    public class ConnectionAcceptEventArgs : EventArgs
    {
        string _result;

        public ConnectionAcceptEventArgs(String result)
        {
            this._result = result;
        }

        public String Result
        {
            get { return _result; }
        }
    }

    public class StepEventArgs : EventArgs
    {
        object _step;

        public StepEventArgs(object step)
        {
            this._step = step;
        }

        public object Step
        {
            get { return _step; }
        }
    }

    public delegate void ConnectionRequestEventHandler(object sender, ConnectionRequestEventArgs e);
    public delegate void ConnectionAcceptEventHandler(object sender, ConnectionAcceptEventArgs e);
    public delegate void StepEventHandler(object sender, StepEventArgs e);
}
