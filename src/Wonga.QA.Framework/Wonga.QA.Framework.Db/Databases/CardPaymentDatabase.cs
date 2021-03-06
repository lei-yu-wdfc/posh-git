#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wonga.QA.Framework.Db.CardPayment
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="CardPayment")]
	public partial class CardPaymentDatabase : DbDatabase<CardPaymentDatabase>
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertMSSQLDeploy(MSSQLDeploy instance);
    partial void UpdateMSSQLDeploy(MSSQLDeploy instance);
    partial void DeleteMSSQLDeploy(MSSQLDeploy instance);
    partial void InsertScheduleEntity(ScheduleEntity instance);
    partial void UpdateScheduleEntity(ScheduleEntity instance);
    partial void DeleteScheduleEntity(ScheduleEntity instance);
    partial void InsertServiceLoginEntity(ServiceLoginEntity instance);
    partial void UpdateServiceLoginEntity(ServiceLoginEntity instance);
    partial void DeleteServiceLoginEntity(ServiceLoginEntity instance);
    #endregion
		
		public CardPaymentDatabase(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CardPaymentDatabase(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CardPaymentDatabase(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CardPaymentDatabase(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
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
		
		public System.Data.Linq.Table<ScheduleEntity> Schedules
		{
			get
			{
				return this.GetTable<ScheduleEntity>();
			}
		}
		
		public System.Data.Linq.Table<ServiceLoginEntity> ServiceLogins
		{
			get
			{
				return this.GetTable<ServiceLoginEntity>();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="cardpayment.Schedules")]
	public partial class ScheduleEntity : DbEntity<ScheduleEntity>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ScheduleId;
		
		private System.Guid _ExternalId;
		
		private System.DateTime _ScheduleDate;
		
		private int _ServiceLoginId;
		
		private System.DateTime _CreatedOn;
		
		private EntityRef<ServiceLoginEntity> _ServiceLoginEntity;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnScheduleIdChanging(int value);
    partial void OnScheduleIdChanged();
    partial void OnExternalIdChanging(System.Guid value);
    partial void OnExternalIdChanged();
    partial void OnScheduleDateChanging(System.DateTime value);
    partial void OnScheduleDateChanged();
    partial void OnServiceLoginIdChanging(int value);
    partial void OnServiceLoginIdChanged();
    partial void OnCreatedOnChanging(System.DateTime value);
    partial void OnCreatedOnChanged();
    #endregion
		
		public ScheduleEntity()
		{
			this._ServiceLoginEntity = default(EntityRef<ServiceLoginEntity>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScheduleId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ScheduleId
		{
			get
			{
				return this._ScheduleId;
			}
			set
			{
				if ((this._ScheduleId != value))
				{
					this.OnScheduleIdChanging(value);
					this.SendPropertyChanging();
					this._ScheduleId = value;
					this.SendPropertyChanged("ScheduleId");
					this.OnScheduleIdChanged();
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScheduleDate", DbType="Date NOT NULL")]
		public System.DateTime ScheduleDate
		{
			get
			{
				return this._ScheduleDate;
			}
			set
			{
				if ((this._ScheduleDate != value))
				{
					this.OnScheduleDateChanging(value);
					this.SendPropertyChanging();
					this._ScheduleDate = value;
					this.SendPropertyChanged("ScheduleDate");
					this.OnScheduleDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServiceLoginId", DbType="Int NOT NULL")]
		public int ServiceLoginId
		{
			get
			{
				return this._ServiceLoginId;
			}
			set
			{
				if ((this._ServiceLoginId != value))
				{
					if (this._ServiceLoginEntity.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnServiceLoginIdChanging(value);
					this.SendPropertyChanging();
					this._ServiceLoginId = value;
					this.SendPropertyChanged("ServiceLoginId");
					this.OnServiceLoginIdChanged();
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
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FK_Schedules_ServiceLogins", Storage="_ServiceLoginEntity", ThisKey="ServiceLoginId", OtherKey="ServiceLoginId", IsForeignKey=true)]
		public ServiceLoginEntity ServiceLoginEntity
		{
			get
			{
				return this._ServiceLoginEntity.Entity;
			}
			set
			{
				ServiceLoginEntity previousValue = this._ServiceLoginEntity.Entity;
				if (((previousValue != value) 
							|| (this._ServiceLoginEntity.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._ServiceLoginEntity.Entity = null;
						previousValue.Schedules.Remove(this);
					}
					this._ServiceLoginEntity.Entity = value;
					if ((value != null))
					{
						value.Schedules.Add(this);
						this._ServiceLoginId = value.ServiceLoginId;
					}
					else
					{
						this._ServiceLoginId = default(int);
					}
					this.SendPropertyChanged("ServiceLoginEntity");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="cardpayment.ServiceLogins")]
	public partial class ServiceLoginEntity : DbEntity<ServiceLoginEntity>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ServiceLoginId;
		
		private System.Guid _ExternalId;
		
		private string _UserName;
		
		private string _Password;
		
		private bool _IsPrimary;
		
		private System.DateTime _CreatedOn;
		
		private EntitySet<ScheduleEntity> _Schedules;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnServiceLoginIdChanging(int value);
    partial void OnServiceLoginIdChanged();
    partial void OnExternalIdChanging(System.Guid value);
    partial void OnExternalIdChanged();
    partial void OnUserNameChanging(string value);
    partial void OnUserNameChanged();
    partial void OnPasswordChanging(string value);
    partial void OnPasswordChanged();
    partial void OnIsPrimaryChanging(bool value);
    partial void OnIsPrimaryChanged();
    partial void OnCreatedOnChanging(System.DateTime value);
    partial void OnCreatedOnChanged();
    #endregion
		
		public ServiceLoginEntity()
		{
			this._Schedules = new EntitySet<ScheduleEntity>(new Action<ScheduleEntity>(this.attach_Schedules), new Action<ScheduleEntity>(this.detach_Schedules));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServiceLoginId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ServiceLoginId
		{
			get
			{
				return this._ServiceLoginId;
			}
			set
			{
				if ((this._ServiceLoginId != value))
				{
					this.OnServiceLoginIdChanging(value);
					this.SendPropertyChanging();
					this._ServiceLoginId = value;
					this.SendPropertyChanged("ServiceLoginId");
					this.OnServiceLoginIdChanged();
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._UserName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Password", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsPrimary", DbType="Bit NOT NULL")]
		public bool IsPrimary
		{
			get
			{
				return this._IsPrimary;
			}
			set
			{
				if ((this._IsPrimary != value))
				{
					this.OnIsPrimaryChanging(value);
					this.SendPropertyChanging();
					this._IsPrimary = value;
					this.SendPropertyChanged("IsPrimary");
					this.OnIsPrimaryChanged();
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
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FK_Schedules_ServiceLogins", Storage="_Schedules", ThisKey="ServiceLoginId", OtherKey="ServiceLoginId", DeleteRule="NO ACTION")]
		public EntitySet<ScheduleEntity> Schedules
		{
			get
			{
				return this._Schedules;
			}
			set
			{
				this._Schedules.Assign(value);
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
		
		private void attach_Schedules(ScheduleEntity entity)
		{
			this.SendPropertyChanging();
			entity.ServiceLoginEntity = this;
		}
		
		private void detach_Schedules(ScheduleEntity entity)
		{
			this.SendPropertyChanging();
			entity.ServiceLoginEntity = null;
		}
	}
}
#pragma warning restore 1591
