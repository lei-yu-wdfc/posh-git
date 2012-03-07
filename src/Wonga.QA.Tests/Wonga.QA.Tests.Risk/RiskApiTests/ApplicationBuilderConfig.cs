﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Risk.RiskApiTests
{
	/// <summary>
	/// helper class to setup application builder
	/// </summary>
	public class ApplicationBuilderConfig
	{
		public IovationMockResponse IovationBlackBox { get; private set; }

		public ApplicationDecisionStatusEnum ExpectedDecisionStatus { get; private set; }

		public ApplicationBuilderConfig(IovationMockResponse iovationBlackBox = IovationMockResponse.Allow, ApplicationDecisionStatusEnum expectedDecisionStatus = ApplicationDecisionStatusEnum.Accepted)
		{
			IovationBlackBox = iovationBlackBox;
			ExpectedDecisionStatus = expectedDecisionStatus;
		}

		public ApplicationBuilderConfig(ApplicationDecisionStatusEnum expectedDecisionStatus)
			: this(IovationMockResponse.Allow, expectedDecisionStatus)
		{

		}
	}
}