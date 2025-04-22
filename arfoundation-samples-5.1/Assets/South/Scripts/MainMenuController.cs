using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour, IPointerDownHandler
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject buttonsContainer; // Objeto padre que contiene todos los botones principales
    [SerializeField] private GameObject casualModePanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Escenas")]
    [SerializeField] private string infiniteModeScene;
    [SerializeField] private string aimModeScene;
    [SerializeField] private string casualModeScene1;
    [SerializeField] private string casualModeScene2;
    [SerializeField] private string casualModeScene3;

    [Header("Configuración de Cierre")]
    [SerializeField] private RectTransform[] panelSafeAreas; // Áreas que no deben cerrar el panel

    private void Start()
    {
        CloseAllPanels();
        ShowMainButtons();
    }

    // Implementación para detectar clicks fuera de los paneles
    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsAnyPanelActive() && IsClickOutsideSafeAreas(eventData))
        {
            CloseAllPanels();
            ShowMainButtons();
        }
    }

    private bool IsAnyPanelActive()
    {
        return (casualModePanel != null && casualModePanel.activeSelf) ||
               (settingsPanel != null && settingsPanel.activeSelf);
    }

    private bool IsClickOutsideSafeAreas(PointerEventData eventData)
    {
        foreach (var safeArea in panelSafeAreas)
        {
            if (safeArea != null && RectTransformUtility.RectangleContainsScreenPoint(safeArea, eventData.position, eventData.pressEventCamera))
            {
                return false;
            }
        }
        return true;
    }

    private void CloseAllPanels()
    {
        if (casualModePanel != null) casualModePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    private void ShowMainButtons()
    {
        if (buttonsContainer != null)
        {
            buttonsContainer.SetActive(true);
        }
    }

    private void HideMainButtons()
    {
        if (buttonsContainer != null)
        {
            buttonsContainer.SetActive(false);
        }
    }

    #region Botones Principales
    public void OnCasualModeButton()
    {
        HideMainButtons();
        OpenPanel(casualModePanel);
    }

    public void OnInfiniteModeButton()
    {
        if (!string.IsNullOrEmpty(infiniteModeScene))
        {
            SceneManager.LoadScene(infiniteModeScene);
        }
    }

    public void OnAimModeButton()
    {
        if (!string.IsNullOrEmpty(aimModeScene))
        {
            SceneManager.LoadScene(aimModeScene);
        }
    }

    public void OnExitModeButton()
    {
        // Esto cierra el juego en una build
        Application.Quit();

        // Esto es útil para probar en el editor (solo funciona en el editor de Unity)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    public void OnSettingsButton()
    {
        HideMainButtons();
        OpenPanel(settingsPanel);
    }

    private void OpenPanel(GameObject panel)
    {
        CloseAllPanels();
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }
    #endregion

    #region Botones del Panel Casual Mode
    public void OnCasualMode1Button()
    {
        if (!string.IsNullOrEmpty(casualModeScene1))
        {
            SceneManager.LoadScene(casualModeScene1);
        }
    }

    public void OnCasualMode2Button()
    {
        if (!string.IsNullOrEmpty(casualModeScene2))
        {
            SceneManager.LoadScene(casualModeScene2);
        }
    }

    public void OnCasualMode3Button()
    {
        if (!string.IsNullOrEmpty(casualModeScene3))
        {
            SceneManager.LoadScene(casualModeScene3);
        }
    }
    #endregion
}