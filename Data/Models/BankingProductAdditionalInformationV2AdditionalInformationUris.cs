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
    /// BankingProductAdditionalInformationV2AdditionalInformationUris
    /// </summary>
    [DataContract]
        public partial class BankingProductAdditionalInformationV2AdditionalInformationUris :  IEquatable<BankingProductAdditionalInformationV2AdditionalInformationUris>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BankingProductAdditionalInformationV2AdditionalInformationUris" /> class.
        /// </summary>
        /// <param name="description">Display text providing more information about the document URI.</param>
        /// <param name="additionalInfoUri">The URI describing the additional information (required).</param>
        public BankingProductAdditionalInformationV2AdditionalInformationUris(string description = default(string), string additionalInfoUri = default(string))
        {
            // to ensure "additionalInfoUri" is required (not null)
            // Greg - Macquarie was missing
            //if (additionalInfoUri == null)
            //{
            //    throw new InvalidDataException("additionalInfoUri is a required property for BankingProductAdditionalInformationV2AdditionalInformationUris and cannot be null");
            //}
            //else
            //{
            this.AdditionalInfoUri = additionalInfoUri;
            //}
            this.Description = description;
        }
        
        /// <summary>
        /// Display text providing more information about the document URI
        /// </summary>
        /// <value>Display text providing more information about the document URI</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// The URI describing the additional information
        /// </summary>
        /// <value>The URI describing the additional information</value>
        [DataMember(Name="additionalInfoUri", EmitDefaultValue=false)]
        public string AdditionalInfoUri { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BankingProductAdditionalInformationV2AdditionalInformationUris {\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
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
            return this.Equals(input as BankingProductAdditionalInformationV2AdditionalInformationUris);
        }

        /// <summary>
        /// Returns true if BankingProductAdditionalInformationV2AdditionalInformationUris instances are equal
        /// </summary>
        /// <param name="input">Instance of BankingProductAdditionalInformationV2AdditionalInformationUris to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BankingProductAdditionalInformationV2AdditionalInformationUris input)
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
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
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
