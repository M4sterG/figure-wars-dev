using UnityEngine;
using UnityEngine.UI;

namespace frame8.ScrollRectItemsAdapter.Classic.Util
{
    /// <summary>Added to a GameObject that has a Button component, it loads <see cref="sceneName"/> scene when the button is clicked</summary>
    public class CLoadSceneOnClick : MonoBehaviour
    {
        /// <summary>The scene to load on click</summary>
        public string sceneName;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(LoadScene);
        }

        private void LoadScene()
        {
#if UNITY_5_3_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
#else
				Application.LoadLevel(sceneName);
#endif
        }

        private int GetIdxToLoad(int curIdx, int numScenes, int incr)
        { return ((curIdx + numScenes) + incr) % numScenes; }
    }
}