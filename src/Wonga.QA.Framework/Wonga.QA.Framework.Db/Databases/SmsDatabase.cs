#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wonga.QA.Framework.Db.Sms
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Sms")]
	public partial class SmsDatabase : DbDatabase<SmsDatabase>
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertMSSQLDeploy(MSSQLDeploy instance);
    partial void UpdateMSSQLDeploy(MSSQLDeploy instance);
    partial void DeleteMSSQLDeploy(MSSQLDeploy instance);
    partial void InsertSmsMessageEntity(SmsMessageEntity instance);
    partial void UpdateSmsMessageEntity(SmsMessageEntity instance);
    partial void DeleteSmsMessageEntity(SmsMessageEntity instance);
    #endregion
		
		public SmsDatabase(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SmsDatabase(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SmsDatabase(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SmsDatabase(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<MSSQLDeploy> MSSQLDeploys
		{
			get
			{
				return this.GetTable<MSSQLDeploy>();
			}
		}
		
		public System.Data.Linq.Table<SmsMessageEntity> SmsMessages
		{
			get
			{
				return this.GetTable<SmsMessageEntity>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.MSSQLDeploy")]
	public partial class MSSQLDeploy : DbEntity<MSSQLDeploy>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private System.DateTime _Date;
		
		private string _Name;
		
		private string _MD5;
		
		private System.Nullable<int> _Revision;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnDateChanging(System.DateTime value);
    partial void OnDateChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnMD5Changing(string value);
    partial void OnMD5Changed();
    partial void OnRevisionChanging(System.Nullable<int> value);
    partial void OnRevisionChanged();
    #endregion
		
		public MSSQLDeploy()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="DateTime NOT NULL")]
		public System.DateTime Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this.OnDateChanging(value);
					this.SendPropertyChanging();
					this._Date = value;
					this.SendPropertyChanged("Date");
					this.OnDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MD5", DbType="VarChar(32) NOT NULL", CanBeNull=false)]
		public string MD5
		{
			get
			{
				return this._MD5;
			}
			set
			{
				if ((this._MD5 != value))
				{
					this.OnMD5Changing(value);
					this.SendPropertyChanging();
					this._MD5 = value;
					this.SendPropertyChanged("MD5");
					this.OnMD5Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Revision", DbType="Int")]
		public System.Nullable<int> Revision
		{
			get
			{
				return this._Revision;
			}
			set
			{
				if ((this._Revision != value))
				{
					this.OnRevisionChanging(value);
					this.SendPropertyChanging();
					this._Revision = value;
					this.SendPropertyChanged("Revision");
					this.OnRevisionChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="sms.SmsMessages")]
	public partial class SmsMessageEntity : DbEntity<SmsMessageEntity>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _SmsMessageId;
		
		private System.Guid _ExternalId;
		
		private string _ReturnAddress;
		
		private int _Status;
		
		private string _ErrorMessage;
		
		private System.DateTime _CreatedOn;
		
		private string _MobilePhoneNumber;
		
		private string _MessageText;
		
		private string _ServiceMsgId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnSmsMessageIdChanging(int value);
    partial void OnSmsMessageIdChanged();
    partial void OnExternalIdChanging(System.Guid value);
    partial void OnExternalIdChanged();
    partial void OnReturnAddressChanging(string value);
    partial void OnReturnAddressChanged();
    partial void OnStatusChanging(int value);
    partial void OnStatusChanged();
    partial void OnErrorMessageChanging(string value);
    partial void OnErrorMessageChanged();
    partial void OnCreatedOnChanging(System.DateTime value);
    partial void OnCreatedOnChanged();
    partial void OnMobilePhoneNumberChanging(string value);
    partial void OnMobilePhoneNumberChanged();
    partial void OnMessageTextChanging(string value);
    partial void OnMessageTextChanged();
    partial void OnServiceMsgIdChanging(string value);
    partial void OnServiceMsgIdChanged();
    #endregion
		
		public SmsMessageEntity()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SmsMessageId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int SmsMessageId
		{
			get
			{
				return this._SmsMessageId;
			}
			set
			{
				if ((this._SmsMessageId != value))
				{
					this.OnSmsMessageIdChanging(value);
					this.SendPropertyChanging();
					this._SmsMessageId = value;
					this.SendPropertyChanged("SmsMessageId");
					this.OnSmsMessageIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExternalId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid ExternalId
		{
			get
			{
				return this._ExternalId;
			}
			set
			{
				if ((this._ExternalId != value))
				{
					this.OnExternalIdChanging(value);
					this.SendPropertyChanging();
					this._ExternalId = value;
					this.SendPropertyChanged("ExternalId");
					this.OnExternalIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReturnAddress", DbType="NVarChar(500)")]
		public string ReturnAddress
		{
			get
			{
				return this._ReturnAddress;
			}
			set
			{
				if ((this._ReturnAddress != value))
				{
					this.OnReturnAddressChanging(value);
					this.SendPropertyChanging();
					this._ReturnAddress = value;
					this.SendPropertyChanged("ReturnAddress");
					this.OnReturnAddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="Int NOT NULL")]
		public int Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorMessage", DbType="NVarChar(255)")]
		public string ErrorMessage
		{
			get
			{
				return this._ErrorMessage;
			}
			set
			{
				if ((this._ErrorMessage != value))
				{
					this.OnErrorMessageChanging(value);
					this.SendPropertyChanging();
					this._ErrorMessage = value;
					this.SendPropertyChanged("ErrorMessage");
					this.OnErrorMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedOn", DbType="DateTime NOT NULL")]
		public System.DateTime CreatedOn
		{
			get
			{
				return this._CreatedOn;
			}
			set
			{
				if ((this._CreatedOn != value))
				{
					this.OnCreatedOnChanging(value);
					this.SendPropertyChanging();
					this._CreatedOn = value;
					this.SendPropertyChanged("CreatedOn");
					this.OnCreatedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MobilePhoneNumber", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string MobilePhoneNumber
		{
			get
			{
				return this._MobilePhoneNumber;
			}
			set
			{
				if ((this._MobilePhoneNumber != value))
				{
					this.OnMobilePhoneNumberChanging(value);
					this.SendPropertyChanging();
					this._MobilePhoneNumber = value;
					this.SendPropertyChanged("MobilePhoneNumber");
					this.OnMobilePhoneNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MessageText", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string MessageText
		{
			get
			{
				return this._MessageText;
			}
			set
			{
				if ((this._MessageText != value))
				{
					this.OnMessageTextChanging(value);
					this.SendPropertyChanging();
					this._MessageText = value;
					this.SendPropertyChanged("MessageText");
					this.OnMessageTextChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServiceMsgId", DbType="VarChar(50)")]
		public string ServiceMsgId
		{
			get
			{
				return this._ServiceMsgId;
			}
			set
			{
				if ((this._ServiceMsgId != value))
				{
					this.OnServiceMsgIdChanging(value);
					this.SendPropertyChanging();
					this._ServiceMsgId = value;
					this.SendPropertyChanged("ServiceMsgId");
					this.OnServiceMsgIdChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591