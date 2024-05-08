using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

namespace TheSpaceRoles
{
    public class ScrollerP
    {
        public static List<ScrollerP> scrollers = new List<ScrollerP>();

        public string ScrollName;
        public Vector3 ScrollPosition;
        public Vector3 StartPos;
        public Vector3 EndPos;
        public Transform Target;
        public Transform parent_;
        public GameObject scroll;
        public GameObject scrollbar;
        public GameObject scroller;
        public PassiveButton scrollbar_passiveButton;
        public PassiveButton scroller_passiveButton;
        public bool mouse_on_bar = false;
        public float size_y;

        public ScrollerP(string Scrollname, ref GameObject mover_parent,ref GameObject parent,Vector3 startPos,Vector3 endPos,Vector3 Pos, float size_y,float target_y)
        {








            Target = mover_parent.transform;
            parent_ = parent.transform;
            ScrollPosition = Pos;
            StartPos = startPos;
            EndPos = endPos;
            this.size_y = size_y;

            targetStartPos_y = target_y;
            ScrollName = Scrollname;


            //var renderer= new SpriteRenderer();
            //renderer.sprite = Sprites.GetSpriteFromResources("ui.scrollbar.png",225);

            //scroll = new GameObject(Scrollname);
            //scroll.gameObject.transform.parent = parent_.transform;
            /*scroll.layer = HudManager.Instance.gameObject.layer;
            var scrollbar = scroll.AddComponent<Scrollbar>();

            SpriteRenderer screnderer = new();
            renderer.sprite = Sprites.GetSpriteFromResources("ui.scrollbar.png", 225);

            renderer.color = Helper.ColorEditHSV(renderer.color, v: -0.3f);
            scrollbar.CachedZ = 0.1f;
            var box = scrollbar.gameObject.AddComponent<BoxCollider2D>();
            box.size = renderer.bounds.size;
            scrollbar.Colliders = new[] { box };
            scrollbar._CachedZ_k__BackingField = 0.1f;
            scrollbar.graphic = screnderer;
            scrollbar.trackGraphic = renderer;
            var box2 = scrollbar.gameObject.AddComponent<BoxCollider2D>();
            box.size = startPos -( startPos + endPos )/2;
            scrollbar.ClickMask = box2;
            scrollbar.horizontal = false;
            scrollbar.ControllerNav = new();*/



            scroll = new GameObject(Scrollname);
            scroll.transform.parent = parent.transform;
            scroll.transform.localPosition = Vector3.zero;
            //parent_ = scroll.transform;
            scroll.SetActive(true);

            //scrollbar
            scrollbar = new("ScrollBar");
            scrollbar.transform.parent = parent_;
            scrollbar.transform.localPosition = ScrollPosition;
            var renderer = scrollbar.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprites.GetSpriteFromResources("ui.scrollbar.png", 225);
            renderer.color = Helper.ColorEditHSV(renderer.color, v: -0.3f);
            scrollbar.layer = HudManager.Instance.gameObject.layer;

            scrollbar_passiveButton = scrollbar.AddComponent<PassiveButton>();
            var box = scrollbar.AddComponent<BoxCollider2D>();
            box.size = renderer.bounds.size;
            scrollbar_passiveButton.OnMouseOver = new UnityEvent();
            scrollbar_passiveButton.OnClick = new ();
            scrollbar_passiveButton._CachedZ_k__BackingField = 0.1f;
            scrollbar_passiveButton.CachedZ = 0.1f;
            scrollbar_passiveButton.Colliders = new[] { scrollbar.GetComponent<BoxCollider2D>() };

            scrollbar.SetActive(true);
            scrollbar_passiveButton.OnMouseOver.AddListener((System.Action)(() =>
            {
                mouse_on_bar = true;

            }));
            scrollbar_passiveButton.OnClick.AddListener((System.Action)(() =>
            {

            }));




            //scroller
            scroller = new("Scroller");
            scroller.transform.parent = parent_;
            scroller.transform.localPosition = ScrollPosition + new Vector3(0, 2f, -1);
            var scrrenderer = scroller.AddComponent<SpriteRenderer>();
            scrrenderer.sprite = Sprites.GetSpriteFromResources("ui.scroller.png", 450);
            scroller.layer = HudManager.Instance.gameObject.layer;

            var scrbox = scroller.AddComponent<BoxCollider2D>();
            scrbox.size = scrrenderer.bounds.size;
            scroller_passiveButton = scroller.AddComponent<PassiveButton>();
            scroller_passiveButton.OnClick = new();
            scroller_passiveButton.OnMouseOver = new UnityEvent();
            scroller_passiveButton._CachedZ_k__BackingField = 0.1f;
            scroller_passiveButton.CachedZ = 0.1f;
            scroller_passiveButton.Colliders = new[] { scroller.GetComponent<BoxCollider2D>() };

            scroller_passiveButton.OnMouseOver.AddListener((System.Action)(() =>
            {
                mouse_on_bar = true;

            }));
            scroller_passiveButton.OnClick.AddListener((System.Action)(() =>
            {
                mouse_on_bar = true;
                bar_dragging = true;

            }));
            scroller.SetActive(true);

            scrollers.Add(this);

        }
        public float targetStartPos_y = 0;
        private static Vector3 GetMouse => Camera.allCameras.First(x => x.name == "UI Camera").ScreenToWorldPoint(Input.mousePosition);
        public bool bar_dragging=false;
        public float scroll_pos = 0;
        public float ac = 0.2f;
        public void Update()
        {
            try
            {

                if (mouse_on_bar)
                {
                    if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                    {
                        bar_dragging = true;
                    }
                    mouse_on_bar = false;
                }

                if (bar_dragging)
                {
                    if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)))
                    {

                        bar_dragging = true;
                        scroll_pos = (-GetMouse.y + 2) / 4;
                        if (scroll_pos < 0)
                        {
                            scroll_pos = 0;
                        }
                        else if (scroll_pos > 1)
                        {
                            scroll_pos = 1;
                        }
                    }
                    else
                    {
                        bar_dragging = false;
                    }
                }
                if (Helper.InArea(GetMouse, StartPos, EndPos))
                {
                    scroll_pos += Input.mouseScrollDelta.y * ac / size_y;


                }
                if (scroll_pos < 0)
                {
                    scroll_pos = 0;
                }
                else if (scroll_pos > 1)
                {
                    scroll_pos = 1;
                }

                Target.transform.position = new Vector3(Target.transform.position.x, targetStartPos_y + scroll_pos * (size_y), Target.transform.position.z);
                


                scroller.transform.localPosition =   new Vector3(ScrollPosition.x, ScrollPosition.y + 2 - (scroll_pos * 4), ScrollPosition.z - 1);



            }
            catch (System.Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class _hudupdate
    {
        public static void Postfix()
        {
            foreach(var scroll in ScrollerP.scrollers)
            {
                try
                {

                    if (scroll?.parent_?.transform?.parent?.gameObject?.active != null || scroll.parent_.transform.parent.gameObject.active)
                    {
                        if (scroll?.parent_?.transform?.parent?.gameObject?.active != null || scroll.parent_.transform.parent.parent.gameObject.active)
                        {

                            scroll.Update();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.Error(ex.Message);

                }
            }
        }
    }
}
