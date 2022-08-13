using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
public class SpineTemp : MonoBehaviour
{
    public SkeletonAnimation skelAnim;

    public SpineAtlasAsset atlasAsset;

    public Sprite[] arrWeaponSprite;
    private Coroutine routine;

    float scale = 1.5f;
    Shader attachmentshader;
    [SpineSlot] public string WeaponSlot;
    Slot EquipSlot;
    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        attachmentshader = Shader.Find("Spine/Skeleton");
        EquipSlot = skelAnim.skeleton.FindSlot(WeaponSlot);
        this.SwapRandomWeapon();
        // this.skelAnim.AnimationState.SetAnimation(0, "attack", false);//.TimeScale = 1.2f;
    }
    RegionAttachment attachment;
    private void SwapRandomWeapon()
    {
        var _sprite = this.arrWeaponSprite[Random.Range(0, arrWeaponSprite.Length)];

        attachment = _sprite.ToRegionAttachmentPMAClone(attachmentshader);

        attachment.SetScale(scale, scale);
        attachment.UpdateOffset();

        EquipSlot.Attachment = attachment;
    }

    private int attackCount = 0;

    private void AnimationState_Complete(Spine.TrackEntry trackEntry)
    {
        Debug.LogFormat("{0}", trackEntry.Animation.Name);
        if (trackEntry.Animation.Name == "attack")
        {
            attackCount++;
            if (this.attackCount > 10)
            {
                this.attackCount = 0;
                if (this.routine != null)
                    StopCoroutine(this.routine);
                this.skelAnim.AnimationState.SetAnimation(0, "idle", true).TimeScale = 1;

                this.routine = StartCoroutine(this.WaitForSeconds(3f, () =>
                {
                    this.SwapRandomWeapon();
                    this.skelAnim.AnimationState.SetAnimation(0, "attack", false);//.TimeScale = 1.2f;
                }));
            }
            else
            {
                this.skelAnim.AnimationState.SetAnimation(0, "attack", false);//.TimeScale = 1.2f;
            }
        }
    }

    private IEnumerator WaitForSeconds(float t, System.Action onComple)
    {
        yield return new WaitForSeconds(t);
        onComple();
    }

    // Update is called once per frame
    void Update()
    {
        skelAnim.skeleton.FindSlot("weapon").Attachment = attachment;
        if (Input.GetKeyDown(KeyCode.A))
        {
            SwapRandomWeapon();
        }
    }

}
