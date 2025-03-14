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
    /// BankingProductV4CardArt
    /// </summary>
    [DataContract]
        public partial class BankingProductV4CardArt :  IEquatable<BankingProductV4CardArt>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BankingProductV4CardArt" /> class.
        /// </summary>
        /// <param name="title">Display label for the specific image.</param>
        /// <param name="imageUri">URI reference to a PNG, JPG or GIF image with proportions defined by ISO 7810 ID-1 and width no greater than 512 pixels. The URI reference may be a link or url-encoded data URI according to **[[RFC2397]](#nref-RFC2397)** (required).</param>
        public BankingProductV4CardArt(string title = default(string), string imageUri = default(string))
        {
            // to ensure "imageUri" is required (not null)
            // Greg - Macquarie was missing
            //if (imageUri == null)
            //{
            //    throw new InvalidDataException("imageUri is a required property for BankingProductV4CardArt and cannot be null");
            //}
            //else
            //{
                this.ImageUri = imageUri;
            //}
            this.Title = title;
        }
        
        /// <summary>
        /// Display label for the specific image
        /// </summary>
        /// <value>Display label for the specific image</value>
        [DataMember(Name="title", EmitDefaultValue=false)]
        public string Title { get; set; }

        /// <summary>
        /// URI reference to a PNG, JPG or GIF image with proportions defined by ISO 7810 ID-1 and width no greater than 512 pixels. The URI reference may be a link or url-encoded data URI according to **[[RFC2397]](#nref-RFC2397)**
        /// </summary>
        /// <value>URI reference to a PNG, JPG or GIF image with proportions defined by ISO 7810 ID-1 and width no greater than 512 pixels. The URI reference may be a link or url-encoded data URI according to **[[RFC2397]](#nref-RFC2397)**</value>
        [DataMember(Name="imageUri", EmitDefaultValue=false)]
        public string ImageUri { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BankingProductV4CardArt {\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  ImageUri: ").Append(ImageUri).Append("\n");
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
            return this.Equals(input as BankingProductV4CardArt);
        }

        /// <summary>
        /// Returns true if BankingProductV4CardArt instances are equal
        /// </summary>
        /// <param name="input">Instance of BankingProductV4CardArt to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BankingProductV4CardArt input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Title == input.Title ||
                    (this.Title != null &&
                    this.Title.Equals(input.Title))
                ) && 
                (
                    this.ImageUri == input.ImageUri ||
                    (this.ImageUri != null &&
                    this.ImageUri.Equals(input.ImageUri))
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
                if (this.Title != null)
                    hashCode = hashCode * 59 + this.Title.GetHashCode();
                if (this.ImageUri != null)
                    hashCode = hashCode * 59 + this.ImageUri.GetHashCode();
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
