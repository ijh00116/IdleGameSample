//using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;

namespace BlackTree
{
    public class IapManager : IStoreListener
    {
        private IStoreController storeController;//구매과정 제어함수 제공
        private IExtensionProvider storeExtensionProvider; //여러플랫폼을 위한 확장처리 제공

        public bool IsInitialized => storeController != null && storeExtensionProvider != null;

        Action PurchaseCallback;

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("유니티IAP 초기화 성공");
            storeController = controller;
            storeExtensionProvider = extensions;

        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("유니티IAP 초기화 실패");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogWarning( string.Format("구매 실패-,{0},{1}", product.definition.id, failureReason));
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            /*
            뒤끝 영수증 검증 처리
            */

            // 구매 성공한 제품에 대한 id 체크하여 그에맞는 보상 
            // A consumable product has been purchased by this user.
            for (int i = 0; i < InGameDataTableManager.shopTableList.iap.Count; i++)
            {
                if (string.Equals(args.purchasedProduct.definition.id, InGameDataTableManager.shopTableList.iap[i].iap_idx.ToString(), System.StringComparison.Ordinal))
                {
                    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                }
            }
            //// 영수증 검증에 성공한 경우
            //if (validation.IsSuccess())
            //{

            //}
            //// 영수증 검증에 실패한 경우 
            //else
            //{
            //    // Or ... an unknown product has been purchased by this user. Fill in additional products here....
            //    Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            //}

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 

            PurchaseCallback = null;
            return PurchaseProcessingResult.Complete;
        }

        

        public void InitUnityIAP()
        {
            if (IsInitialized)
                return;
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            for(int i=0; i<InGameDataTableManager.shopTableList.iap.Count; i++)
            {
                builder.AddProduct(InGameDataTableManager.shopTableList.iap[i].iap_idx.ToString(), ProductType.Consumable, new IDs() { {InGameDataTableManager.shopTableList.iap[i].ios,AppleAppStore.Name },
                    {InGameDataTableManager.shopTableList.iap[i].google,GooglePlay.Name } });
            }
           
            UnityPurchasing.Initialize(this, builder);
        }

        public float GetPaymentPrice(string iapId)
        {
            if (IsInitialized==false)
                return -1.0f;
            float price= (float)storeController.products.WithID(iapId).metadata.localizedPrice;
            return price;
        }

        public void Purchase(string productId,System.Action callback)
        {
            if (!IsInitialized)
                return;

            if (PurchaseCallback != null)
                return;

            PurchaseCallback = callback;
            var product = storeController.products.WithID(productId);
            if(product!=null&&product.availableToPurchase)
            {
                Debug.Log(string.Format("구매시도-{0}",product.definition.id));
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log(string.Format("구매시도불가-{0}", product.definition.id));
            }
        }

        //Ios나 안드로이드 아닐때 이전 구매 복구
        public void RestorePurchase()
        {
            if (!IsInitialized) return;
            if(Application.platform==RuntimePlatform.IPhonePlayer||Application.platform==RuntimePlatform.OSXPlayer)
            {
                Debug.Log("구매복구 시도");

                var appleExt = storeExtensionProvider.GetExtension<IAppleExtensions>();

                appleExt.RestoreTransactions(o=>Debug.Log(o.ToString()));
            }
        }
        public bool HadPurchased(string productId)
        {
            if (!IsInitialized) return false;

            var product = storeController.products.WithID(productId);
            if(product!=null)
            {
                return product.hasReceipt;
            }

            return false;
        }

        public string targetProductId;
      
    }

}
