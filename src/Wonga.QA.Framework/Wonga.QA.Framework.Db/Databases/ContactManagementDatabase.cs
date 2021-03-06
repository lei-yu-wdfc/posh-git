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

namespace Wonga.QA.Framework.Db.ContactManagement
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="ContactManagement")]
	public partial class ContactManagementDatabase : DbDatabase<ContactManagementDatabase>
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertBusinessAddressEntity(BusinessAddressEntity instance);
    partial void UpdateBusinessAddressEntity(BusinessAddressEntity instance);
    partial void DeleteBusinessAddressEntity(BusinessAddressEntity instance);
    partial void InsertDirectorOrganisationMappingEntity(DirectorOrganisationMappingEntity instance);
    partial void UpdateDirectorOrganisationMappingEntity(DirectorOrganisationMappingEntity instance);
    partial void DeleteDirectorOrganisationMappingEntity(DirectorOrganisationMappingEntity instance);
    partial void InsertMSSQLDeploy(MSSQLDeploy instance);
    partial void UpdateMSSQLDeploy(MSSQLDeploy instance);
    partial void DeleteMSSQLDeploy(MSSQLDeploy instance);
    partial void InsertOrganisationDetailEntity(OrganisationDetailEntity instance);
    partial void UpdateOrganisationDetailEntity(OrganisationDetailEntity instance);
    partial void DeleteOrganisationDetailEntity(OrganisationDetailEntity instance);
    #endregion
		
		public ContactManagementDatabase(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ContactManagementDatabase(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ContactManagementDatabase(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ContactManagementDatabase(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<BusinessAddressEntity> BusinessAddresses
		{
			get
			{
				return this.GetTable<BusinessAddressEntity>();
			}
		}
		
		public System.Data.Linq.Table<DirectorOrganisationMappingEntity> DirectorOrganisationMappings
		{
			get
			{
				return this.GetTable<DirectorOrganisationMappingEntity>();
			}
		}
		
		public System.Data.Linq.Table<MSSQLDeploy> MSSQLDeploys
		{
			get
			{
				return this.GetTable<MSSQLDeploy>();
			}
		}
		
		public System.Data.Linq.Table<OrganisationDetailEntity> OrganisationDetails
		{
			get
			{
				return this.GetTable<OrganisationDetailEntity>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="ContactManagement.BusinessAddresses")]
	public partial class BusinessAddressEntity : DbEntity<BusinessAddressEntity>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _BusinessAddressId;
		
		private System.Guid _OrganisationId;
		
		private System.Guid _ExternalId;
		
		private int _AddressType;
		
		private string _Building;
		
		private string _Street;
		
		private string _City;
		
		private string _County;
		
		private string _CountryCode;
		
		private string _Postcode;
		
		private System.Nullable<System.DateTime> _CreatedOn;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnBusinessAddressIdChanging(int value);
    partial void OnBusinessAddressIdChanged();
    partial void OnOrganisationIdChanging(System.Guid value);
    partial void OnOrganisationIdChanged();
    partial void OnExternalIdChanging(System.Guid value);
    partial void OnExternalIdChanged();
    partial void OnAddressTypeChanging(int value);
    partial void OnAddressTypeChanged();
    partial void OnBuildingChanging(string value);
    partial void OnBuildingChanged();
    partial void OnStreetChanging(string value);
    partial void OnStreetChanged();
    partial void OnCityChanging(string value);
    partial void OnCityChanged();
    partial void OnCountyChanging(string value);
    partial void OnCountyChanged();
    partial void OnCountryCodeChanging(string value);
    partial void OnCountryCodeChanged();
    partial void OnPostcodeChanging(string value);
    partial void OnPostcodeChanged();
    partial void OnCreatedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnCreatedOnChanged();
    #endregion
		
		public BusinessAddressEntity()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BusinessAddressId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int BusinessAddressId
		{
			get
			{
				return this._BusinessAddressId;
			}
			set
			{
				if ((this._BusinessAddressId != value))
				{
					this.OnBusinessAddressIdChanging(value);
					this.SendPropertyChanging();
					this._BusinessAddressId = value;
					this.SendPropertyChanged("BusinessAddressId");
					this.OnBusinessAddressIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OrganisationId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid OrganisationId
		{
			get
			{
				return this._OrganisationId;
			}
			set
			{
				if ((this._OrganisationId != value))
				{
					this.OnOrganisationIdChanging(value);
					this.SendPropertyChanging();
					this._OrganisationId = value;
					this.SendPropertyChanged("OrganisationId");
					this.OnOrganisationIdChanged();
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AddressType", DbType="Int NOT NULL")]
		public int AddressType
		{
			get
			{
				return this._AddressType;
			}
			set
			{
				if ((this._AddressType != value))
				{
					this.OnAddressTypeChanging(value);
					this.SendPropertyChanging();
					this._AddressType = value;
					this.SendPropertyChanged("AddressType");
					this.OnAddressTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Building", DbType="NVarChar(255)")]
		public string Building
		{
			get
			{
				return this._Building;
			}
			set
			{
				if ((this._Building != value))
				{
					this.OnBuildingChanging(value);
					this.SendPropertyChanging();
					this._Building = value;
					this.SendPropertyChanged("Building");
					this.OnBuildingChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Street", DbType="NVarChar(255)")]
		public string Street
		{
			get
			{
				return this._Street;
			}
			set
			{
				if ((this._Street != value))
				{
					this.OnStreetChanging(value);
					this.SendPropertyChanging();
					this._Street = value;
					this.SendPropertyChanged("Street");
					this.OnStreetChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_City", DbType="NVarChar(255)")]
		public string City
		{
			get
			{
				return this._City;
			}
			set
			{
				if ((this._City != value))
				{
					this.OnCityChanging(value);
					this.SendPropertyChanging();
					this._City = value;
					this.SendPropertyChanged("City");
					this.OnCityChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_County", DbType="NVarChar(255)")]
		public string County
		{
			get
			{
				return this._County;
			}
			set
			{
				if ((this._County != value))
				{
					this.OnCountyChanging(value);
					this.SendPropertyChanging();
					this._County = value;
					this.SendPropertyChanged("County");
					this.OnCountyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CountryCode", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string CountryCode
		{
			get
			{
				return this._CountryCode;
			}
			set
			{
				if ((this._CountryCode != value))
				{
					this.OnCountryCodeChanging(value);
					this.SendPropertyChanging();
					this._CountryCode = value;
					this.SendPropertyChanged("CountryCode");
					this.OnCountryCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Postcode", DbType="NVarChar(255)")]
		public string Postcode
		{
			get
			{
				return this._Postcode;
			}
			set
			{
				if ((this._Postcode != value))
				{
					this.OnPostcodeChanging(value);
					this.SendPropertyChanging();
					this._Postcode = value;
					this.SendPropertyChanged("Postcode");
					this.OnPostcodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> CreatedOn
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="ContactManagement.DirectorOrganisationMappings")]
	public partial class DirectorOrganisationMappingEntity : DbEntity<DirectorOrganisationMappingEntity>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _DirectorOrganisationMappingId;
		
		private int _DirectorLevel;
		
		private System.Nullable<System.DateTime> _CreatedOn;
		
		private System.Guid _OrganisationId;
		
		private System.Guid _AccountId;
		
		private int _Title;
		
		private string _Forename;
		
		private string _Surname;
		
		private string _Email;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDirectorOrganisationMappingIdChanging(int value);
    partial void OnDirectorOrganisationMappingIdChanged();
    partial void OnDirectorLevelChanging(int value);
    partial void OnDirectorLevelChanged();
    partial void OnCreatedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnCreatedOnChanged();
    partial void OnOrganisationIdChanging(System.Guid value);
    partial void OnOrganisationIdChanged();
    partial void OnAccountIdChanging(System.Guid value);
    partial void OnAccountIdChanged();
    partial void OnTitleChanging(int value);
    partial void OnTitleChanged();
    partial void OnForenameChanging(string value);
    partial void OnForenameChanged();
    partial void OnSurnameChanging(string value);
    partial void OnSurnameChanged();
    partial void OnEmailChanging(string value);
    partial void OnEmailChanged();
    #endregion
		
		public DirectorOrganisationMappingEntity()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DirectorOrganisationMappingId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int DirectorOrganisationMappingId
		{
			get
			{
				return this._DirectorOrganisationMappingId;
			}
			set
			{
				if ((this._DirectorOrganisationMappingId != value))
				{
					this.OnDirectorOrganisationMappingIdChanging(value);
					this.SendPropertyChanging();
					this._DirectorOrganisationMappingId = value;
					this.SendPropertyChanged("DirectorOrganisationMappingId");
					this.OnDirectorOrganisationMappingIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DirectorLevel", DbType="Int NOT NULL")]
		public int DirectorLevel
		{
			get
			{
				return this._DirectorLevel;
			}
			set
			{
				if ((this._DirectorLevel != value))
				{
					this.OnDirectorLevelChanging(value);
					this.SendPropertyChanging();
					this._DirectorLevel = value;
					this.SendPropertyChanged("DirectorLevel");
					this.OnDirectorLevelChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> CreatedOn
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OrganisationId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid OrganisationId
		{
			get
			{
				return this._OrganisationId;
			}
			set
			{
				if ((this._OrganisationId != value))
				{
					this.OnOrganisationIdChanging(value);
					this.SendPropertyChanging();
					this._OrganisationId = value;
					this.SendPropertyChanged("OrganisationId");
					this.OnOrganisationIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AccountId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid AccountId
		{
			get
			{
				return this._AccountId;
			}
			set
			{
				if ((this._AccountId != value))
				{
					this.OnAccountIdChanging(value);
					this.SendPropertyChanging();
					this._AccountId = value;
					this.SendPropertyChanged("AccountId");
					this.OnAccountIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="Int NOT NULL")]
		public int Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Forename", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Forename
		{
			get
			{
				return this._Forename;
			}
			set
			{
				if ((this._Forename != value))
				{
					this.OnForenameChanging(value);
					this.SendPropertyChanging();
					this._Forename = value;
					this.SendPropertyChanged("Forename");
					this.OnForenameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Surname", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Surname
		{
			get
			{
				return this._Surname;
			}
			set
			{
				if ((this._Surname != value))
				{
					this.OnSurnameChanging(value);
					this.SendPropertyChanging();
					this._Surname = value;
					this.SendPropertyChanged("Surname");
					this.OnSurnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Email", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this.OnEmailChanging(value);
					this.SendPropertyChanging();
					this._Email = value;
					this.SendPropertyChanged("Email");
					this.OnEmailChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="ContactManagement.OrganisationDetails")]
	public partial class OrganisationDetailEntity : DbEntity<OrganisationDetailEntity>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _OrganisationDetailsId;
		
		private System.Guid _OrganisationId;
		
		private System.Nullable<System.DateTime> _CreatedOn;
		
		private string _OrganisationName;
		
		private string _RegisteredNumber;
		
		private string _VatNumber;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnOrganisationDetailsIdChanging(int value);
    partial void OnOrganisationDetailsIdChanged();
    partial void OnOrganisationIdChanging(System.Guid value);
    partial void OnOrganisationIdChanged();
    partial void OnCreatedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnCreatedOnChanged();
    partial void OnOrganisationNameChanging(string value);
    partial void OnOrganisationNameChanged();
    partial void OnRegisteredNumberChanging(string value);
    partial void OnRegisteredNumberChanged();
    partial void OnVatNumberChanging(string value);
    partial void OnVatNumberChanged();
    #endregion
		
		public OrganisationDetailEntity()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OrganisationDetailsId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int OrganisationDetailsId
		{
			get
			{
				return this._OrganisationDetailsId;
			}
			set
			{
				if ((this._OrganisationDetailsId != value))
				{
					this.OnOrganisationDetailsIdChanging(value);
					this.SendPropertyChanging();
					this._OrganisationDetailsId = value;
					this.SendPropertyChanged("OrganisationDetailsId");
					this.OnOrganisationDetailsIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OrganisationId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid OrganisationId
		{
			get
			{
				return this._OrganisationId;
			}
			set
			{
				if ((this._OrganisationId != value))
				{
					this.OnOrganisationIdChanging(value);
					this.SendPropertyChanging();
					this._OrganisationId = value;
					this.SendPropertyChanged("OrganisationId");
					this.OnOrganisationIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> CreatedOn
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OrganisationName", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string OrganisationName
		{
			get
			{
				return this._OrganisationName;
			}
			set
			{
				if ((this._OrganisationName != value))
				{
					this.OnOrganisationNameChanging(value);
					this.SendPropertyChanging();
					this._OrganisationName = value;
					this.SendPropertyChanged("OrganisationName");
					this.OnOrganisationNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RegisteredNumber", DbType="NVarChar(8) NOT NULL", CanBeNull=false)]
		public string RegisteredNumber
		{
			get
			{
				return this._RegisteredNumber;
			}
			set
			{
				if ((this._RegisteredNumber != value))
				{
					this.OnRegisteredNumberChanging(value);
					this.SendPropertyChanging();
					this._RegisteredNumber = value;
					this.SendPropertyChanged("RegisteredNumber");
					this.OnRegisteredNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VatNumber", DbType="NVarChar(20)")]
		public string VatNumber
		{
			get
			{
				return this._VatNumber;
			}
			set
			{
				if ((this._VatNumber != value))
				{
					this.OnVatNumberChanging(value);
					this.SendPropertyChanging();
					this._VatNumber = value;
					this.SendPropertyChanged("VatNumber");
					this.OnVatNumberChanged();
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
