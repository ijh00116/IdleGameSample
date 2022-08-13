using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.Common;

namespace BlackTree
{
    public enum BundleType
    {
        None,
        Tables,
    }

    public class ResourceLoader : MonoSingleton<ResourceLoader>
    {
        public class LoadedResource
        {
            public UnityEngine.Object mResource;
            public int mReferencedCount;

            public LoadedResource(UnityEngine.Object resource)
            {
                mResource = resource;
                mReferencedCount = 1;
            }
        }

        Dictionary<string, LoadedResource> loadedResources = new Dictionary<string, LoadedResource>();
        Dictionary<string, ResourceRequest> inProgressOperations = new Dictionary<string, ResourceRequest>();

        public IEnumerator Load<T>(string ResourceName, System.Action<UnityEngine.Object> onComplete)
        {
            while (inProgressOperations.ContainsKey(ResourceName))
                yield return null;

            if (loadedResources.ContainsKey(ResourceName))
            {
                var resource = loadedResources[ResourceName];
                if (resource != null && resource.mResource != null)
                {
                    resource.mReferencedCount++;
                    if (onComplete != null)
                        onComplete(resource.mResource);
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogError("Resource Is Already Loaded. But Actual Data Not Loaded");
#endif
                }
            }
            else
            {
                ResourceRequest request = Resources.LoadAsync(ResourceName, typeof(T));
                inProgressOperations.Add(ResourceName, request);

                yield return request;

                inProgressOperations.Remove(ResourceName);

                if (request.asset != null)
                {
                    var resource = new LoadedResource(request.asset);
                    loadedResources.Add(ResourceName, resource);

                    if (onComplete != null)
                        onComplete(request.asset);
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogError("Resource load is complete. But Data is Null ::"+ ResourceName.ToString());
#endif
                }
            }
        }

        public float GetProgress(string resourceName)
        {
            if (loadedResources.ContainsKey(resourceName))
                return 1.0f;

            ResourceRequest request;
            inProgressOperations.TryGetValue(resourceName, out request);
            if (request != null)
            {
                return request.progress;
            }

            return 0.0f;
        }

        public void Unload(string resourceName)
        {
            return;
        }

        public void UnloadAll()
        {
            loadedResources.Clear();
            Resources.UnloadUnusedAssets();
        }

#if UNITY_EDITOR
        void LogNotUnloadingResources()
        {
            foreach (var resPair in loadedResources)
            {
                var res = resPair.Value;
                if (res != null && res.mResource != null)
                {
                    Debug.LogWarningFormat("Not unloading resource : \"{0}\", RefCount : {1}", resPair.Key, res.mReferencedCount);
                }
            }
        }

        protected override void Release()
        {
            UnloadAll();
            LogNotUnloadingResources();
        }
#endif
    }

}
