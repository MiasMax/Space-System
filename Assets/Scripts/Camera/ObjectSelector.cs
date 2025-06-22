using UnityEngine;

public static class ObjectSelector
{
    public static Transform HandleClickSelection(Camera camera, Transform currentSelection)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.transform;
            }
            else
            {
                return null; // clic dans le vide -> d�s�lection
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return null; // �chap -> d�s�lection
        }

        return currentSelection; // pas de changement
    }

}