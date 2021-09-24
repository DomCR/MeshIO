//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeshIO.GLTF.Schema.V2 {
    using System.Linq;
    using System.Runtime.Serialization;
    
    
    public class GltfCamera {
        
        /// <summary>
        /// Backing field for Orthographic.
        /// </summary>
        private GltfCameraOrthographic _orthographic;
        
        /// <summary>
        /// Backing field for Perspective.
        /// </summary>
        private GltfCameraPerspective _perspective;
        
        /// <summary>
        /// Backing field for Type.
        /// </summary>
        private TypeEnum _type;
        
        /// <summary>
        /// Backing field for Name.
        /// </summary>
        private string _name;
        
        /// <summary>
        /// Backing field for Extensions.
        /// </summary>
        private System.Collections.Generic.Dictionary<string, object> _extensions;
        
        /// <summary>
        /// Backing field for Extras.
        /// </summary>
        private GltfExtras _extras;
        
        /// <summary>
        /// An orthographic camera containing properties to create an orthographic projection matrix.
        /// </summary>
        [Newtonsoft.Json.JsonPropertyAttribute("orthographic")]
        public GltfCameraOrthographic Orthographic {
            get {
                return this._orthographic;
            }
            set {
                this._orthographic = value;
            }
        }
        
        /// <summary>
        /// A perspective camera containing properties to create a perspective projection matrix.
        /// </summary>
        [Newtonsoft.Json.JsonPropertyAttribute("perspective")]
        public GltfCameraPerspective Perspective {
            get {
                return this._perspective;
            }
            set {
                this._perspective = value;
            }
        }
        
        /// <summary>
        /// Specifies if the camera uses a perspective or orthographic projection.
        /// </summary>
        [Newtonsoft.Json.JsonConverterAttribute(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        [Newtonsoft.Json.JsonRequiredAttribute()]
        [Newtonsoft.Json.JsonPropertyAttribute("type")]
        public TypeEnum Type {
            get {
                return this._type;
            }
            set {
                this._type = value;
            }
        }
        
        /// <summary>
        /// The user-defined name of this object.
        /// </summary>
        [Newtonsoft.Json.JsonPropertyAttribute("name")]
        public string Name {
            get {
                return this._name;
            }
            set {
                this._name = value;
            }
        }
        
        /// <summary>
        /// Dictionary object with extension-specific objects.
        /// </summary>
        [Newtonsoft.Json.JsonPropertyAttribute("extensions")]
        public System.Collections.Generic.Dictionary<string, object> Extensions {
            get {
                return this._extensions;
            }
            set {
                this._extensions = value;
            }
        }
        
        /// <summary>
        /// Application-specific data.
        /// </summary>
        [Newtonsoft.Json.JsonPropertyAttribute("extras")]
        public GltfExtras Extras {
            get {
                return this._extras;
            }
            set {
                this._extras = value;
            }
        }
        
        public bool ShouldSerializeOrthographic() {
            return ((_orthographic == null) 
                        == false);
        }
        
        public bool ShouldSerializePerspective() {
            return ((_perspective == null) 
                        == false);
        }
        
        public bool ShouldSerializeName() {
            return ((_name == null) 
                        == false);
        }
        
        public bool ShouldSerializeExtensions() {
            return ((_extensions == null) 
                        == false);
        }
        
        public bool ShouldSerializeExtras() {
            return ((_extras == null) 
                        == false);
        }
        
        public enum TypeEnum {
            
            perspective,
            
            orthographic,
        }
    }
}