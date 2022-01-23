using CharaCustom;
using KKAPI.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MakerSortIconReplacer
{
    public class MakerSortWindowHelper
    {
        public Button alphaAscButton { get; set; }
        public Button alphaDescButton { get; set; }
        public Button dateAscButton { get; set; }
        public Button dateDescButton { get; set; }
        
        public CustomCharaWindow Window { get; set; }
        public CustomClothesWindow ClothesWindow { get; set; }

        private bool alphaSortAsc = true;
        private bool dateSortAsc = false;

        public void SetupButtons(CustomCharaWindow window, GameObject buttonToggleBase, GameObject buttonSortMethodBase)
        {
            Window = window;
            LoadIcons();

            alphaAscButton = SetupButton(buttonSortMethodBase, AlphaAscIcon, true);
            alphaAscButton.gameObject.name = "AlphaAscButton";
            alphaAscButton.transform.Translate(new Vector3(-50, 0));
            alphaAscButton.gameObject.SetActive(true);

            alphaDescButton = SetupButton(buttonSortMethodBase, AlphaDescIcon, true);
            alphaDescButton.gameObject.name = "AlphaDescButton";
            alphaDescButton.transform.Translate(new Vector3(-50, 0));
            alphaDescButton.gameObject.SetActive(false);

            dateAscButton = SetupButton(buttonSortMethodBase, DateAscIcon, false);
            dateAscButton.gameObject.name = "DateAscButton";
            dateAscButton.gameObject.SetActive(false);

            dateDescButton = SetupButton(buttonSortMethodBase, DateDescIcon, false, true);
            dateDescButton.gameObject.name = "DateDescButton";
            dateDescButton.gameObject.SetActive(true);

            buttonToggleBase.SetActive(false);
            buttonSortMethodBase.SetActive(false);

            Window.cscChara.SetTopLine();
        }

        public void SetupClothesButtons(CustomClothesWindow window, GameObject buttonToggleBase, GameObject buttonSortMethodBase)
        {
            ClothesWindow = window;
            LoadIcons();

            alphaAscButton = SetupButton(buttonSortMethodBase, AlphaAscIcon, true);
            alphaAscButton.gameObject.name = "AlphaAscButton";
            alphaAscButton.transform.Translate(new Vector3(-50, 0));
            alphaAscButton.gameObject.SetActive(true);

            alphaDescButton = SetupButton(buttonSortMethodBase, AlphaDescIcon, true);
            alphaDescButton.gameObject.name = "AlphaDescButton";
            alphaDescButton.transform.Translate(new Vector3(-50, 0));
            alphaDescButton.gameObject.SetActive(false);

            dateAscButton = SetupButton(buttonSortMethodBase, DateAscIcon, false);
            dateAscButton.gameObject.name = "DateAscButton";
            dateAscButton.gameObject.SetActive(false);

            dateDescButton = SetupButton(buttonSortMethodBase, DateDescIcon, false, true);
            dateDescButton.gameObject.name = "DateDescButton";
            dateDescButton.gameObject.SetActive(true);

            buttonToggleBase.SetActive(false);
            buttonSortMethodBase.SetActive(false);

            ClothesWindow.cscClothes.SetTopLine();
        }


        private Button SetupButton(GameObject buttonBase, byte[] icon, bool alphaButton, bool firstSelection = false)
        {
            GameObject newToggle = GameObject.Instantiate(buttonBase, buttonBase.transform.parent);
            Image newImage = newToggle.GetComponent<Image>();

            Button newButton = newToggle.GetComponent<Button>();
            newButton.onClick.ActuallyRemoveAllListeners();

            newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(36, 35);

            UnityEngine.Object.DestroyImmediate(newImage);
            newImage = newToggle.AddComponent<Image>();
            Texture2D tex = new Texture2D(36, 36);
            tex.LoadImage(icon);

            newImage.sprite = Sprite.Create(tex, new Rect(0, 0, 36, 36), new Vector2(0.5f, 0.5f));
            newButton.targetGraphic = newImage;
            newButton.transition = Selectable.Transition.ColorTint;
            ColorBlock cb = newButton.colors;
            if (!firstSelection)
                cb.normalColor = new Color(233, 227, 222);
            else
                cb.normalColor = Color.green;
            newButton.colors = cb;

            newButton.onClick.AddListener(() => {
                HandleSortChange(alphaButton);
            });

            return newButton;
        }

        private void HandleSortChange(bool alpha)
        {
            if (Window)
                HandleCharaSortChange(alpha);
            else
                HandleClothesSortChange(alpha);
        }

        private void HandleCharaSortChange(bool alpha)
        { 
            if (Window.sortType == 0 && alpha)
            {
                Window.sortType = 1;
                Window.sortOrder = alphaSortAsc ? 0 : 1;
            }
            else if (Window.sortType == 1 && !alpha)
            {
                Window.sortType = 0;
                Window.sortOrder = dateSortAsc ? 0 : 1;
            }
            else if (alpha)
            {
                alphaSortAsc = !alphaSortAsc;
                Window.sortOrder = alphaSortAsc ? 0 : 1;
            }
            else
            {
                dateSortAsc = !dateSortAsc;
                Window.sortOrder = dateSortAsc ? 0 : 1;
            }

            alphaAscButton.gameObject.SetActive(false);
            alphaDescButton.gameObject.SetActive(false);
            dateAscButton.gameObject.SetActive(false);
            dateDescButton.gameObject.SetActive(false);

            if (dateSortAsc)
                dateAscButton.gameObject.SetActive(true);
            else
                dateDescButton.gameObject.SetActive(true);

            if (alphaSortAsc)
                alphaAscButton.gameObject.SetActive(true);
            else
                alphaDescButton.gameObject.SetActive(true);

            if (Window.sortType == 0)
            {
                ColorBlock cb = dateAscButton.colors;
                cb.normalColor = Color.green;
                dateAscButton.colors = cb;
                dateDescButton.colors = cb;
                ColorBlock cbNonSelect = alphaAscButton.colors;
                cbNonSelect.normalColor = new Color(233, 227, 222);
                alphaAscButton.colors = cbNonSelect;
                alphaDescButton.colors = cbNonSelect;
            }
            else
            {
                ColorBlock cb = alphaAscButton.colors;
                cb.normalColor = Color.green;
                alphaAscButton.colors = cb;
                alphaDescButton.colors = cb;
                ColorBlock cbNonSelect = dateAscButton.colors;
                cbNonSelect.normalColor = new Color(233, 227, 222);
                dateAscButton.colors = cbNonSelect;
                dateDescButton.colors = cbNonSelect;
            }
           

            Window.Sort();
        }

        private void HandleClothesSortChange(bool alpha)
        {
            if (ClothesWindow.sortType == 0 && alpha)
            {
                ClothesWindow.sortType = 1;
                ClothesWindow.sortOrder = alphaSortAsc ? 0 : 1;
            }
            else if (ClothesWindow.sortType == 1 && !alpha)
            {
                ClothesWindow.sortType = 0;
                ClothesWindow.sortOrder = dateSortAsc ? 0 : 1;
            }
            else if (alpha)
            {
                alphaSortAsc = !alphaSortAsc;
                ClothesWindow.sortOrder = alphaSortAsc ? 0 : 1;
            }
            else
            {
                dateSortAsc = !dateSortAsc;
                ClothesWindow.sortOrder = dateSortAsc ? 0 : 1;
            }

            alphaAscButton.gameObject.SetActive(false);
            alphaDescButton.gameObject.SetActive(false);
            dateAscButton.gameObject.SetActive(false);
            dateDescButton.gameObject.SetActive(false);

            if (dateSortAsc)
                dateAscButton.gameObject.SetActive(true);
            else
                dateDescButton.gameObject.SetActive(true);

            if (alphaSortAsc)
                alphaAscButton.gameObject.SetActive(true);
            else
                alphaDescButton.gameObject.SetActive(true);

            if (ClothesWindow.sortType == 0)
            {
                ColorBlock cb = dateAscButton.colors;
                cb.normalColor = Color.green;
                dateAscButton.colors = cb;
                dateDescButton.colors = cb;
                ColorBlock cbNonSelect = alphaAscButton.colors;
                cbNonSelect.normalColor = new Color(233, 227, 222);
                alphaAscButton.colors = cbNonSelect;
                alphaDescButton.colors = cbNonSelect;
            }
            else
            {
                ColorBlock cb = alphaAscButton.colors;
                cb.normalColor = Color.green;
                alphaAscButton.colors = cb;
                alphaDescButton.colors = cb;
                ColorBlock cbNonSelect = dateAscButton.colors;
                cbNonSelect.normalColor = new Color(233, 227, 222);
                dateAscButton.colors = cbNonSelect;
                dateDescButton.colors = cbNonSelect;
            }


            ClothesWindow.Sort();
        }

        private static byte[] AlphaAscIcon;
        private static byte[] AlphaDescIcon;
        private static byte[] DateAscIcon;
        private static byte[] DateDescIcon;
        private static void LoadIcons()
        {
#if HS2
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"HS2_MakerSortIconReplacer.resources.AlphaAsc.png"))
#else
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AI_MakerSortIconReplacer.resources.AlphaAsc.png"))
#endif
            {
                AlphaAscIcon = new byte[stream.Length];
                stream.Read(AlphaAscIcon, 0, AlphaAscIcon.Length);
            }

#if HS2
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"HS2_MakerSortIconReplacer.resources.AlphaDesc.png"))
#else
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AI_MakerSortIconReplacer.resources.AlphaDesc.png"))
#endif
            {
                AlphaDescIcon = new byte[stream.Length];
                stream.Read(AlphaDescIcon, 0, AlphaDescIcon.Length);
            }

#if HS2
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"HS2_MakerSortIconReplacer.resources.DateAsc.png"))
#else
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AI_MakerSortIconReplacer.resources.DateAsc.png"))
#endif
            {
                DateAscIcon = new byte[stream.Length];
                stream.Read(DateAscIcon, 0, DateAscIcon.Length);
            }

#if HS2
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"HS2_MakerSortIconReplacer.resources.DateDesc.png"))
#else
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AI_MakerSortIconReplacer.resources.DateDesc.png"))
#endif
            {
                DateDescIcon = new byte[stream.Length];
                stream.Read(DateDescIcon, 0, DateDescIcon.Length);
            }

        }
    }
}
