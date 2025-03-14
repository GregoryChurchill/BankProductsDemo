/*
 * CDR Banking API
 *
 * Consumer Data Standards APIs created by the Data Standards Body (DSB), with the Data Standards Chair as the decision maker to meet the needs of the Consumer Data Right
 *
 * OpenAPI spec version: 1.27.0
 * Contact: contact@consumerdatastandards.gov.au
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BankingProductsData.Models
{
    /// <summary>
    /// Defines the criteria and conditions for which a rate applies
    /// </summary>
    [DataContract]
        public partial class BankingProductRateTierV3 :  IEquatable<BankingProductRateTierV3>, IValidatableObject
    {
        /// <summary>
        /// The unit of measure that applies to the tierValueMinimum and tierValueMaximum values e.g. a **DOLLAR** amount. **PERCENT** (in the case of loan-to-value ratio or LVR). Tier term period representing a discrete number of **MONTH**&#x27;s or **DAY**&#x27;s (in the case of term deposit tiers)
        /// </summary>
        /// <value>The unit of measure that applies to the tierValueMinimum and tierValueMaximum values e.g. a **DOLLAR** amount. **PERCENT** (in the case of loan-to-value ratio or LVR). Tier term period representing a discrete number of **MONTH**&#x27;s or **DAY**&#x27;s (in the case of term deposit tiers)</value>
        [JsonConverter(typeof(StringEnumConverter))]
                public enum UnitOfMeasureEnum
        {
            /// <summary>
            /// Enum DAY for value: DAY
            /// </summary>
            [EnumMember(Value = "DAY")]
            DAY = 1,
            /// <summary>
            /// Enum DOLLAR for value: DOLLAR
            /// </summary>
            [EnumMember(Value = "DOLLAR")]
            DOLLAR = 2,
            /// <summary>
            /// Enum MONTH for value: MONTH
            /// </summary>
            [EnumMember(Value = "MONTH")]
            MONTH = 3,
            /// <summary>
            /// Enum PERCENT for value: PERCENT
            /// </summary>
            [EnumMember(Value = "PERCENT")]
            PERCENT = 4        }
        /// <summary>
        /// The unit of measure that applies to the tierValueMinimum and tierValueMaximum values e.g. a **DOLLAR** amount. **PERCENT** (in the case of loan-to-value ratio or LVR). Tier term period representing a discrete number of **MONTH**&#x27;s or **DAY**&#x27;s (in the case of term deposit tiers)
        /// </summary>
        /// <value>The unit of measure that applies to the tierValueMinimum and tierValueMaximum values e.g. a **DOLLAR** amount. **PERCENT** (in the case of loan-to-value ratio or LVR). Tier term period representing a discrete number of **MONTH**&#x27;s or **DAY**&#x27;s (in the case of term deposit tiers)</value>
        [DataMember(Name="unitOfMeasure", EmitDefaultValue=false)]
        public UnitOfMeasureEnum UnitOfMeasure { get; set; }
        /// <summary>
        /// The method used to calculate the amount to be applied using one or more tiers. A single rate may be applied to the entire balance or each applicable tier rate is applied to the portion of the balance that falls into that tier (referred to as &#x27;bands&#x27; or &#x27;steps&#x27;)
        /// </summary>
        /// <value>The method used to calculate the amount to be applied using one or more tiers. A single rate may be applied to the entire balance or each applicable tier rate is applied to the portion of the balance that falls into that tier (referred to as &#x27;bands&#x27; or &#x27;steps&#x27;)</value>
        [JsonConverter(typeof(StringEnumConverter))]
                public enum RateApplicationMethodEnum
        {
            /// <summary>
            /// Enum PERTIER for value: PER_TIER
            /// </summary>
            [EnumMember(Value = "PER_TIER")]
            PERTIER = 1,
            /// <summary>
            /// Enum WHOLEBALANCE for value: WHOLE_BALANCE
            /// </summary>
            [EnumMember(Value = "WHOLE_BALANCE")]
            WHOLEBALANCE = 2        }
        /// <summary>
        /// The method used to calculate the amount to be applied using one or more tiers. A single rate may be applied to the entire balance or each applicable tier rate is applied to the portion of the balance that falls into that tier (referred to as &#x27;bands&#x27; or &#x27;steps&#x27;)
        /// </summary>
        /// <value>The method used to calculate the amount to be applied using one or more tiers. A single rate may be applied to the entire balance or each applicable tier rate is applied to the portion of the balance that falls into that tier (referred to as &#x27;bands&#x27; or &#x27;steps&#x27;)</value>
        [DataMember(Name="rateApplicationMethod", EmitDefaultValue=false)]
        public RateApplicationMethodEnum? RateApplicationMethod { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BankingProductRateTierV3" /> class.
        /// </summary>
        /// <param name="name">A display name for the tier (required).</param>
        /// <param name="unitOfMeasure">The unit of measure that applies to the tierValueMinimum and tierValueMaximum values e.g. a **DOLLAR** amount. **PERCENT** (in the case of loan-to-value ratio or LVR). Tier term period representing a discrete number of **MONTH**&#x27;s or **DAY**&#x27;s (in the case of term deposit tiers) (required).</param>
        /// <param name="minimumValue">The number of tierUnitOfMeasure units that form the lower bound of the tier. The tier should be inclusive of this value (required).</param>
        /// <param name="maximumValue">The number of tierUnitOfMeasure units that form the upper bound of the tier or band. For a tier with a discrete value (as opposed to a range of values e.g. 1 month) this must be the same as tierValueMinimum. Where this is the same as the tierValueMinimum value of the next-higher tier the referenced tier should be exclusive of this value. For example a term deposit of 2 months falls into the upper tier of the following tiers: (1 – 2 months, 2 – 3 months). If absent the tier&#x27;s range has no upper bound..</param>
        /// <param name="rateApplicationMethod">The method used to calculate the amount to be applied using one or more tiers. A single rate may be applied to the entire balance or each applicable tier rate is applied to the portion of the balance that falls into that tier (referred to as &#x27;bands&#x27; or &#x27;steps&#x27;).</param>
        /// <param name="applicabilityConditions">applicabilityConditions.</param>
        /// <param name="additionalInfo">Display text providing more information on the rate tier..</param>
        /// <param name="additionalInfoUri">Link to a web page with more information on this rate tier.</param>
        public BankingProductRateTierV3(string name = default(string), UnitOfMeasureEnum unitOfMeasure = default(UnitOfMeasureEnum), decimal? minimumValue = default(decimal?), decimal? maximumValue = default(decimal?), RateApplicationMethodEnum? rateApplicationMethod = default(RateApplicationMethodEnum?), BankingProductRateCondition applicabilityConditions = default(BankingProductRateCondition), string additionalInfo = default(string), string additionalInfoUri = default(string))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new InvalidDataException("name is a required property for BankingProductRateTierV3 and cannot be null");
            }
            else
            {
                this.Name = name;
            }
            // to ensure "unitOfMeasure" is required (not null)
            if (unitOfMeasure == null)
            {
                throw new InvalidDataException("unitOfMeasure is a required property for BankingProductRateTierV3 and cannot be null");
            }
            else
            {
                this.UnitOfMeasure = unitOfMeasure;
            }
            // to ensure "minimumValue" is required (not null)
            if (minimumValue == null)
            {
                throw new InvalidDataException("minimumValue is a required property for BankingProductRateTierV3 and cannot be null");
            }
            else
            {
                this.MinimumValue = minimumValue;
            }
            this.MaximumValue = maximumValue;
            this.RateApplicationMethod = rateApplicationMethod;
            this.ApplicabilityConditions = applicabilityConditions;
            this.AdditionalInfo = additionalInfo;
            this.AdditionalInfoUri = additionalInfoUri;
        }
        
        /// <summary>
        /// A display name for the tier
        /// </summary>
        /// <value>A display name for the tier</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }


        /// <summary>
        /// The number of tierUnitOfMeasure units that form the lower bound of the tier. The tier should be inclusive of this value
        /// </summary>
        /// <value>The number of tierUnitOfMeasure units that form the lower bound of the tier. The tier should be inclusive of this value</value>
        [DataMember(Name="minimumValue", EmitDefaultValue=false)]
        public decimal? MinimumValue { get; set; }

        /// <summary>
        /// The number of tierUnitOfMeasure units that form the upper bound of the tier or band. For a tier with a discrete value (as opposed to a range of values e.g. 1 month) this must be the same as tierValueMinimum. Where this is the same as the tierValueMinimum value of the next-higher tier the referenced tier should be exclusive of this value. For example a term deposit of 2 months falls into the upper tier of the following tiers: (1 – 2 months, 2 – 3 months). If absent the tier&#x27;s range has no upper bound.
        /// </summary>
        /// <value>The number of tierUnitOfMeasure units that form the upper bound of the tier or band. For a tier with a discrete value (as opposed to a range of values e.g. 1 month) this must be the same as tierValueMinimum. Where this is the same as the tierValueMinimum value of the next-higher tier the referenced tier should be exclusive of this value. For example a term deposit of 2 months falls into the upper tier of the following tiers: (1 – 2 months, 2 – 3 months). If absent the tier&#x27;s range has no upper bound.</value>
        [DataMember(Name="maximumValue", EmitDefaultValue=false)]
        public decimal? MaximumValue { get; set; }


        /// <summary>
        /// Gets or Sets ApplicabilityConditions
        /// </summary>
        [DataMember(Name="applicabilityConditions", EmitDefaultValue=false)]
        public BankingProductRateCondition ApplicabilityConditions { get; set; }

        /// <summary>
        /// Display text providing more information on the rate tier.
        /// </summary>
        /// <value>Display text providing more information on the rate tier.</value>
        [DataMember(Name="additionalInfo", EmitDefaultValue=false)]
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Link to a web page with more information on this rate tier
        /// </summary>
        /// <value>Link to a web page with more information on this rate tier</value>
        [DataMember(Name="additionalInfoUri", EmitDefaultValue=false)]
        public string AdditionalInfoUri { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BankingProductRateTierV3 {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  UnitOfMeasure: ").Append(UnitOfMeasure).Append("\n");
            sb.Append("  MinimumValue: ").Append(MinimumValue).Append("\n");
            sb.Append("  MaximumValue: ").Append(MaximumValue).Append("\n");
            sb.Append("  RateApplicationMethod: ").Append(RateApplicationMethod).Append("\n");
            sb.Append("  ApplicabilityConditions: ").Append(ApplicabilityConditions).Append("\n");
            sb.Append("  AdditionalInfo: ").Append(AdditionalInfo).Append("\n");
            sb.Append("  AdditionalInfoUri: ").Append(AdditionalInfoUri).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as BankingProductRateTierV3);
        }

        /// <summary>
        /// Returns true if BankingProductRateTierV3 instances are equal
        /// </summary>
        /// <param name="input">Instance of BankingProductRateTierV3 to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BankingProductRateTierV3 input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.UnitOfMeasure == input.UnitOfMeasure ||
                    (this.UnitOfMeasure != null &&
                    this.UnitOfMeasure.Equals(input.UnitOfMeasure))
                ) && 
                (
                    this.MinimumValue == input.MinimumValue ||
                    (this.MinimumValue != null &&
                    this.MinimumValue.Equals(input.MinimumValue))
                ) && 
                (
                    this.MaximumValue == input.MaximumValue ||
                    (this.MaximumValue != null &&
                    this.MaximumValue.Equals(input.MaximumValue))
                ) && 
                (
                    this.RateApplicationMethod == input.RateApplicationMethod ||
                    (this.RateApplicationMethod != null &&
                    this.RateApplicationMethod.Equals(input.RateApplicationMethod))
                ) && 
                (
                    this.ApplicabilityConditions == input.ApplicabilityConditions ||
                    (this.ApplicabilityConditions != null &&
                    this.ApplicabilityConditions.Equals(input.ApplicabilityConditions))
                ) && 
                (
                    this.AdditionalInfo == input.AdditionalInfo ||
                    (this.AdditionalInfo != null &&
                    this.AdditionalInfo.Equals(input.AdditionalInfo))
                ) && 
                (
                    this.AdditionalInfoUri == input.AdditionalInfoUri ||
                    (this.AdditionalInfoUri != null &&
                    this.AdditionalInfoUri.Equals(input.AdditionalInfoUri))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.UnitOfMeasure != null)
                    hashCode = hashCode * 59 + this.UnitOfMeasure.GetHashCode();
                if (this.MinimumValue != null)
                    hashCode = hashCode * 59 + this.MinimumValue.GetHashCode();
                if (this.MaximumValue != null)
                    hashCode = hashCode * 59 + this.MaximumValue.GetHashCode();
                if (this.RateApplicationMethod != null)
                    hashCode = hashCode * 59 + this.RateApplicationMethod.GetHashCode();
                if (this.ApplicabilityConditions != null)
                    hashCode = hashCode * 59 + this.ApplicabilityConditions.GetHashCode();
                if (this.AdditionalInfo != null)
                    hashCode = hashCode * 59 + this.AdditionalInfo.GetHashCode();
                if (this.AdditionalInfoUri != null)
                    hashCode = hashCode * 59 + this.AdditionalInfoUri.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
