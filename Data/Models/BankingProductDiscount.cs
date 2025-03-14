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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BankingProductsData.Models
{
    /// <summary>
    /// BankingProductDiscount
    /// </summary>
    [DataContract]
        public partial class BankingProductDiscount :  IEquatable<BankingProductDiscount>, IValidatableObject
    {
        /// <summary>
        /// The type of discount. See the next section for an overview of valid values and their meaning
        /// </summary>
        /// <value>The type of discount. See the next section for an overview of valid values and their meaning</value>
        [JsonConverter(typeof(StringEnumConverter))]
                public enum DiscountTypeEnum
        {
            /// <summary>
            /// Enum BALANCE for value: BALANCE
            /// </summary>
            [EnumMember(Value = "BALANCE")]
            BALANCE = 1,
            /// <summary>
            /// Enum DEPOSITS for value: DEPOSITS
            /// </summary>
            [EnumMember(Value = "DEPOSITS")]
            DEPOSITS = 2,
            /// <summary>
            /// Enum ELIGIBILITYONLY for value: ELIGIBILITY_ONLY
            /// </summary>
            [EnumMember(Value = "ELIGIBILITY_ONLY")]
            ELIGIBILITYONLY = 3,
            /// <summary>
            /// Enum FEECAP for value: FEE_CAP
            /// </summary>
            [EnumMember(Value = "FEE_CAP")]
            FEECAP = 4,
            /// <summary>
            /// Enum PAYMENTS for value: PAYMENTS
            /// </summary>
            [EnumMember(Value = "PAYMENTS")]
            PAYMENTS = 5        }
        /// <summary>
        /// The type of discount. See the next section for an overview of valid values and their meaning
        /// </summary>
        /// <value>The type of discount. See the next section for an overview of valid values and their meaning</value>
        [DataMember(Name="discountType", EmitDefaultValue=false)]
        public DiscountTypeEnum DiscountType { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BankingProductDiscount" /> class.
        /// </summary>
        /// <param name="description">Description of the discount (required).</param>
        /// <param name="discountType">The type of discount. See the next section for an overview of valid values and their meaning (required).</param>
        /// <param name="amount">Dollar value of the discount. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory..</param>
        /// <param name="balanceRate">A discount rate calculated based on a proportion of the balance. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory. Unless noted in additionalInfo, assumes the application and calculation frequency are the same as the corresponding fee.</param>
        /// <param name="transactionRate">A discount rate calculated based on a proportion of a transaction. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory.</param>
        /// <param name="accruedRate">A discount rate calculated based on a proportion of the calculated interest accrued on the account. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory. Unless noted in additionalInfo, assumes the application and calculation frequency are the same as the corresponding fee.</param>
        /// <param name="feeRate">A discount rate calculated based on a proportion of the fee to which this discount is attached. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory. Unless noted in additionalInfo, assumes the application and calculation frequency are the same as the corresponding fee.</param>
        /// <param name="additionalValue">Generic field containing additional information relevant to the [discountType](#tocSproductdiscounttypedoc) specified. Whether mandatory or not is dependent on the value of [discountType](#tocSproductdiscounttypedoc).</param>
        /// <param name="additionalInfo">Display text providing more information on the discount.</param>
        /// <param name="additionalInfoUri">Link to a web page with more information on this discount.</param>
        /// <param name="eligibility">Eligibility constraints that apply to this discount. Mandatory if &#x60;&#x60;discountType&#x60;&#x60; is &#x60;&#x60;ELIGIBILITY_ONLY&#x60;&#x60;..</param>
        public BankingProductDiscount(string description = default(string), DiscountTypeEnum discountType = default(DiscountTypeEnum), string amount = default(string), string balanceRate = default(string), string transactionRate = default(string), string accruedRate = default(string), string feeRate = default(string), string additionalValue = default(string), string additionalInfo = default(string), string additionalInfoUri = default(string), List<BankingProductDiscountEligibility> eligibility = default(List<BankingProductDiscountEligibility>))
        {
            // to ensure "description" is required (not null)
            if (description == null)
            {
                throw new InvalidDataException("description is a required property for BankingProductDiscount and cannot be null");
            }
            else
            {
                this.Description = description;
            }
            // to ensure "discountType" is required (not null)
            if (discountType == null)
            {
                throw new InvalidDataException("discountType is a required property for BankingProductDiscount and cannot be null");
            }
            else
            {
                this.DiscountType = discountType;
            }
            this.Amount = amount;
            this.BalanceRate = balanceRate;
            this.TransactionRate = transactionRate;
            this.AccruedRate = accruedRate;
            this.FeeRate = feeRate;
            this.AdditionalValue = additionalValue;
            this.AdditionalInfo = additionalInfo;
            this.AdditionalInfoUri = additionalInfoUri;
            this.Eligibility = eligibility;
        }
        
        /// <summary>
        /// Description of the discount
        /// </summary>
        /// <value>Description of the discount</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }


        /// <summary>
        /// Dollar value of the discount. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory.
        /// </summary>
        /// <value>Dollar value of the discount. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory.</value>
        [DataMember(Name="amount", EmitDefaultValue=false)]
        public string Amount { get; set; }

        /// <summary>
        /// A discount rate calculated based on a proportion of the balance. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory. Unless noted in additionalInfo, assumes the application and calculation frequency are the same as the corresponding fee
        /// </summary>
        /// <value>A discount rate calculated based on a proportion of the balance. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory. Unless noted in additionalInfo, assumes the application and calculation frequency are the same as the corresponding fee</value>
        [DataMember(Name="balanceRate", EmitDefaultValue=false)]
        public string BalanceRate { get; set; }

        /// <summary>
        /// A discount rate calculated based on a proportion of a transaction. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory
        /// </summary>
        /// <value>A discount rate calculated based on a proportion of a transaction. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory</value>
        [DataMember(Name="transactionRate", EmitDefaultValue=false)]
        public string TransactionRate { get; set; }

        /// <summary>
        /// A discount rate calculated based on a proportion of the calculated interest accrued on the account. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory. Unless noted in additionalInfo, assumes the application and calculation frequency are the same as the corresponding fee
        /// </summary>
        /// <value>A discount rate calculated based on a proportion of the calculated interest accrued on the account. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory. Unless noted in additionalInfo, assumes the application and calculation frequency are the same as the corresponding fee</value>
        [DataMember(Name="accruedRate", EmitDefaultValue=false)]
        public string AccruedRate { get; set; }

        /// <summary>
        /// A discount rate calculated based on a proportion of the fee to which this discount is attached. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory. Unless noted in additionalInfo, assumes the application and calculation frequency are the same as the corresponding fee
        /// </summary>
        /// <value>A discount rate calculated based on a proportion of the fee to which this discount is attached. Note that the currency of the fee discount is expected to be the same as the currency of the fee itself. One of amount, balanceRate, transactionRate, accruedRate and feeRate is mandatory. Unless noted in additionalInfo, assumes the application and calculation frequency are the same as the corresponding fee</value>
        [DataMember(Name="feeRate", EmitDefaultValue=false)]
        public string FeeRate { get; set; }

        /// <summary>
        /// Generic field containing additional information relevant to the [discountType](#tocSproductdiscounttypedoc) specified. Whether mandatory or not is dependent on the value of [discountType](#tocSproductdiscounttypedoc)
        /// </summary>
        /// <value>Generic field containing additional information relevant to the [discountType](#tocSproductdiscounttypedoc) specified. Whether mandatory or not is dependent on the value of [discountType](#tocSproductdiscounttypedoc)</value>
        [DataMember(Name="additionalValue", EmitDefaultValue=false)]
        public string AdditionalValue { get; set; }

        /// <summary>
        /// Display text providing more information on the discount
        /// </summary>
        /// <value>Display text providing more information on the discount</value>
        [DataMember(Name="additionalInfo", EmitDefaultValue=false)]
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Link to a web page with more information on this discount
        /// </summary>
        /// <value>Link to a web page with more information on this discount</value>
        [DataMember(Name="additionalInfoUri", EmitDefaultValue=false)]
        public string AdditionalInfoUri { get; set; }

        /// <summary>
        /// Eligibility constraints that apply to this discount. Mandatory if &#x60;&#x60;discountType&#x60;&#x60; is &#x60;&#x60;ELIGIBILITY_ONLY&#x60;&#x60;.
        /// </summary>
        /// <value>Eligibility constraints that apply to this discount. Mandatory if &#x60;&#x60;discountType&#x60;&#x60; is &#x60;&#x60;ELIGIBILITY_ONLY&#x60;&#x60;.</value>
        [DataMember(Name="eligibility", EmitDefaultValue=false)]
        public List<BankingProductDiscountEligibility> Eligibility { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BankingProductDiscount {\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  DiscountType: ").Append(DiscountType).Append("\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
            sb.Append("  BalanceRate: ").Append(BalanceRate).Append("\n");
            sb.Append("  TransactionRate: ").Append(TransactionRate).Append("\n");
            sb.Append("  AccruedRate: ").Append(AccruedRate).Append("\n");
            sb.Append("  FeeRate: ").Append(FeeRate).Append("\n");
            sb.Append("  AdditionalValue: ").Append(AdditionalValue).Append("\n");
            sb.Append("  AdditionalInfo: ").Append(AdditionalInfo).Append("\n");
            sb.Append("  AdditionalInfoUri: ").Append(AdditionalInfoUri).Append("\n");
            sb.Append("  Eligibility: ").Append(Eligibility).Append("\n");
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
            return this.Equals(input as BankingProductDiscount);
        }

        /// <summary>
        /// Returns true if BankingProductDiscount instances are equal
        /// </summary>
        /// <param name="input">Instance of BankingProductDiscount to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BankingProductDiscount input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.DiscountType == input.DiscountType ||
                    (this.DiscountType != null &&
                    this.DiscountType.Equals(input.DiscountType))
                ) && 
                (
                    this.Amount == input.Amount ||
                    (this.Amount != null &&
                    this.Amount.Equals(input.Amount))
                ) && 
                (
                    this.BalanceRate == input.BalanceRate ||
                    (this.BalanceRate != null &&
                    this.BalanceRate.Equals(input.BalanceRate))
                ) && 
                (
                    this.TransactionRate == input.TransactionRate ||
                    (this.TransactionRate != null &&
                    this.TransactionRate.Equals(input.TransactionRate))
                ) && 
                (
                    this.AccruedRate == input.AccruedRate ||
                    (this.AccruedRate != null &&
                    this.AccruedRate.Equals(input.AccruedRate))
                ) && 
                (
                    this.FeeRate == input.FeeRate ||
                    (this.FeeRate != null &&
                    this.FeeRate.Equals(input.FeeRate))
                ) && 
                (
                    this.AdditionalValue == input.AdditionalValue ||
                    (this.AdditionalValue != null &&
                    this.AdditionalValue.Equals(input.AdditionalValue))
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
                ) && 
                (
                    this.Eligibility == input.Eligibility ||
                    this.Eligibility != null &&
                    input.Eligibility != null &&
                    this.Eligibility.SequenceEqual(input.Eligibility)
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
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.DiscountType != null)
                    hashCode = hashCode * 59 + this.DiscountType.GetHashCode();
                if (this.Amount != null)
                    hashCode = hashCode * 59 + this.Amount.GetHashCode();
                if (this.BalanceRate != null)
                    hashCode = hashCode * 59 + this.BalanceRate.GetHashCode();
                if (this.TransactionRate != null)
                    hashCode = hashCode * 59 + this.TransactionRate.GetHashCode();
                if (this.AccruedRate != null)
                    hashCode = hashCode * 59 + this.AccruedRate.GetHashCode();
                if (this.FeeRate != null)
                    hashCode = hashCode * 59 + this.FeeRate.GetHashCode();
                if (this.AdditionalValue != null)
                    hashCode = hashCode * 59 + this.AdditionalValue.GetHashCode();
                if (this.AdditionalInfo != null)
                    hashCode = hashCode * 59 + this.AdditionalInfo.GetHashCode();
                if (this.AdditionalInfoUri != null)
                    hashCode = hashCode * 59 + this.AdditionalInfoUri.GetHashCode();
                if (this.Eligibility != null)
                    hashCode = hashCode * 59 + this.Eligibility.GetHashCode();
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
