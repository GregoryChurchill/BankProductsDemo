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
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace BankingProductsData.Models
{
    /// <summary>
    /// Defines a condition for the applicability of a tiered rate
    /// </summary>
    [DataContract]
        public partial class BankingProductRateCondition :  IEquatable<BankingProductRateCondition>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BankingProductRateCondition" /> class.
        /// </summary>
        /// <param name="additionalInfo">Display text providing more information on the condition.</param>
        /// <param name="additionalInfoUri">Link to a web page with more information on this condition.</param>
        public BankingProductRateCondition(string additionalInfo = default(string), string additionalInfoUri = default(string))
        {
            this.AdditionalInfo = additionalInfo;
            this.AdditionalInfoUri = additionalInfoUri;
        }
        
        /// <summary>
        /// Display text providing more information on the condition
        /// </summary>
        /// <value>Display text providing more information on the condition</value>
        [DataMember(Name="additionalInfo", EmitDefaultValue=false)]
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Link to a web page with more information on this condition
        /// </summary>
        /// <value>Link to a web page with more information on this condition</value>
        [DataMember(Name="additionalInfoUri", EmitDefaultValue=false)]
        public string AdditionalInfoUri { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BankingProductRateCondition {\n");
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
            return this.Equals(input as BankingProductRateCondition);
        }

        /// <summary>
        /// Returns true if BankingProductRateCondition instances are equal
        /// </summary>
        /// <param name="input">Instance of BankingProductRateCondition to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BankingProductRateCondition input)
        {
            if (input == null)
                return false;

            return 
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
