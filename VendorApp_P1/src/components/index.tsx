import * as React from "react"
import * as ReactDom from "react-dom"

import ProductSearch from "./ProductSearch"

export default class Index {
  public static loadComponent() {
    const productSearchPlaceholder: HTMLElement | null = document.getElementById(
      "product-search"
    )

    if (productSearchPlaceholder !== null) {
      ReactDom.render(
        <ProductSearch />,
        document.getElementById("product-search")
      )
    }
  }
}
