import Swal from "sweetalert2"

/**
 * Binds event triggers to various elements across the website that will fire a swal modal once clicked.
 */
export default class SwalHandler {
  /**
   * Bind onclick evens to various elements on the website that will trigger a
   * specific Swal modal
   */
  public static init() {
    this.bindToAddCartWithNoAuthentication()
    this.bindToFlashMessages()
  }

  /**
   * Binds swal to the AddCart button when on the viewing a product to buy if the user is not signed in.
   * The swal message should instruct the user to sign/login before adding a product to their cart
   */
  public static bindToAddCartWithNoAuthentication() {
    // Find the button to bind to swal
    const addCartButton: Element | null = document.querySelector(
      "#swal-add-cart-no-auth"
    )

    //
    if (addCartButton == null) {
      // In most cases this means the user is not signed in
      return
    }

    // Bind onCliock event to button to fire a swal modal
    addCartButton.addEventListener("click", (e) => {
      Swal.fire({
        title: "Hold Up!",
        icon: "info",
        showConfirmButton: false,
        showCloseButton: true,
        html: `
          <div class="swal-login-buttons">
            <p>You need to be logged in first</p>
            <a href="${
              (window as any).VendorAppLoginUrl
            }"><button class="btn btn-primary">Login</button></a>
            <a href="${
              (window as any).VendorAppRegisterUrl
            }"><button class="btn btn-primary">Register</button></a>
          </div>
          `,
      })
    })
  }

  /**
   * This will search for a FlashMessage elements that will trigger a Swal modal/toast
   * to display when these elements are present.
   */
  public static bindToFlashMessages() {
    // Look for the swal flashmessage element
    const flashSuccessMessage: HTMLElement | null = document.querySelector(
      "#swal-toast-flash-message"
    )
    const flashSuccessMessageModal: HTMLElement | null = document.querySelector(
      "#swal-flash-message"
    )
    const flashErrMessage: HTMLElement | null = document.querySelector(
      "#swal-toast-err-flash-message"
    )

    if (flashSuccessMessage) {
      Swal.fire({
        title: flashSuccessMessage.innerText,
        toast: true,
        icon: "success",
        position: "top",
      })
    }

    if (flashSuccessMessageModal) {
      Swal.fire({
        title: "Congratulations!",
        text: flashSuccessMessageModal.innerText,
        icon: "success",
      })
    }

    if (flashErrMessage) {
      Swal.fire({
        title: flashErrMessage.innerText,
        toast: true,
        icon: "error",
        position: "top",
      })
    }
  }
}
