import ProductSearchComponent from "./components"
import SwalHandler from "./SwalHandler"

class Main {
  /**
   * Used to initialize behaviors by javascript
   */
  public static init() {
    // Load react components
    ProductSearchComponent.loadComponent()
    // setup up sweetalert to fire on various events
    SwalHandler.init()
  }
}

// Initialize behavior when the dom loads
document.addEventListener("DOMContentLoaded", (e) => {
  Main.init()
})
