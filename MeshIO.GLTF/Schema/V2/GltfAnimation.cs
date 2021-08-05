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
    
    
    public class GltfAnimation {
        
        /// <summary>
        /// Backing field for Channels.
        /// </summary>
        private GltfAnimationChannel[] _channels;
        
        /// <summary>
        /// Backing field for Samplers.
        /// </summary>
        private GltfAnimationSampler[] _samplers;
        
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
        /// An array of channels, each of which targets an animation's sampler at a node's property. Different channels of the same animation can't have equal targets.
        /// </summary>
        [Newtonsoft.Json.JsonRequiredAttribute()]
        [Newtonsoft.Json.JsonPropertyAttribute("channels")]
        public GltfAnimationChannel[] Channels {
            get {
                return this._channels;
            }
            set {
                if ((value == null)) {
                    this._channels = value;
                    return;
                }
                if ((value.Length < 1u)) {
                    throw new System.ArgumentException("Array not long enough");
                }
                this._channels = value;
            }
        }
        
        /// <summary>
        /// An array of samplers that combines input and output accessors with an interpolation algorithm to define a keyframe graph (but not its target).
        /// </summary>
        [Newtonsoft.Json.JsonRequiredAttribute()]
        [Newtonsoft.Json.JsonPropertyAttribute("samplers")]
        public GltfAnimationSampler[] Samplers {
            get {
                return this._samplers;
            }
            set {
                if ((value == null)) {
                    this._samplers = value;
                    return;
                }
                if ((value.Length < 1u)) {
                    throw new System.ArgumentException("Array not long enough");
                }
                this._samplers = value;
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
        
        public bool ShouldSerializeChannels() {
            return ((_channels == null) 
                        == false);
        }
        
        public bool ShouldSerializeSamplers() {
            return ((_samplers == null) 
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
    }
}
