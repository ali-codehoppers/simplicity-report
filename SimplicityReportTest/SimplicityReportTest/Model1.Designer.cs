﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace SimplicityReportTest
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class LoggingEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new LoggingEntities object using the connection string found in the 'LoggingEntities' section of the application configuration file.
        /// </summary>
        public LoggingEntities() : base("name=LoggingEntities", "LoggingEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new LoggingEntities object.
        /// </summary>
        public LoggingEntities(string connectionString) : base(connectionString, "LoggingEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new LoggingEntities object.
        /// </summary>
        public LoggingEntities(EntityConnection connection) : base(connection, "LoggingEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Log> Logs
        {
            get
            {
                if ((_Logs == null))
                {
                    _Logs = base.CreateObjectSet<Log>("Logs");
                }
                return _Logs;
            }
        }
        private ObjectSet<Log> _Logs;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Logs EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToLogs(Log log)
        {
            base.AddObject("Logs", log);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="LoggingModel", Name="Log")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Log : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Log object.
        /// </summary>
        /// <param name="logId">Initial value of the LogId property.</param>
        /// <param name="logTime">Initial value of the LogTime property.</param>
        /// <param name="type">Initial value of the Type property.</param>
        /// <param name="message">Initial value of the Message property.</param>
        public static Log CreateLog(global::System.Int32 logId, global::System.DateTime logTime, global::System.String type, global::System.String message)
        {
            Log log = new Log();
            log.LogId = logId;
            log.LogTime = logTime;
            log.Type = type;
            log.Message = message;
            return log;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 LogId
        {
            get
            {
                return _LogId;
            }
            set
            {
                if (_LogId != value)
                {
                    OnLogIdChanging(value);
                    ReportPropertyChanging("LogId");
                    _LogId = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("LogId");
                    OnLogIdChanged();
                }
            }
        }
        private global::System.Int32 _LogId;
        partial void OnLogIdChanging(global::System.Int32 value);
        partial void OnLogIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime LogTime
        {
            get
            {
                return _LogTime;
            }
            set
            {
                OnLogTimeChanging(value);
                ReportPropertyChanging("LogTime");
                _LogTime = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("LogTime");
                OnLogTimeChanged();
            }
        }
        private global::System.DateTime _LogTime;
        partial void OnLogTimeChanging(global::System.DateTime value);
        partial void OnLogTimeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Type
        {
            get
            {
                return _Type;
            }
            set
            {
                OnTypeChanging(value);
                ReportPropertyChanging("Type");
                _Type = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Type");
                OnTypeChanged();
            }
        }
        private global::System.String _Type;
        partial void OnTypeChanging(global::System.String value);
        partial void OnTypeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Message
        {
            get
            {
                return _Message;
            }
            set
            {
                OnMessageChanging(value);
                ReportPropertyChanging("Message");
                _Message = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Message");
                OnMessageChanged();
            }
        }
        private global::System.String _Message;
        partial void OnMessageChanging(global::System.String value);
        partial void OnMessageChanged();

        #endregion
    
    }

    #endregion
    
}
