import React from "react"

import axios, { AxiosResponse } from "axios"

import Container from "react-bootstrap/Container"
import Row from "react-bootstrap/Row"
import Col from "react-bootstrap/Col"
import Button from "react-bootstrap/Button"

import FormControl from "react-bootstrap/FormControl"

import ListGroup from "react-bootstrap/ListGroup"

import Styled from "styled-components"

import BeatLoader from "react-spinners/BeatLoader"

// TODO: This file's out of control, cleanup and split code into other files

// * Interfaces for data recieved from AJAX

/**
 * VendorApp's Catagory Schema
 */
interface IVACatagory {
  /**
   * Catagory's unqiqu id
   */
  id: number
  /**
   * The name of the catagory
   */
  catagoryName: string
  /**
   * The catagory's color theme given in a 6 digit hexidecimal value with a
   * preceding #
   */
  hexValueTheme: string
}

/**
 * The data scheme of a VendorApp Location model
 */
interface IVALocation {
  /**
   * The unique id for the VendorApp Location
   */
  id: number
  /**
   * The name of the location
   */
  name: string
}

/**
 * The data scheme of a VendorApp Product model
 */
interface IVAProduct {
  /**
   * The unique id for the VendorApp Product
   */
  id: number
  /**
   * The name of the product
   */
  name: string
  /**
   * The product's catagory
   */
  catagory: string
  /**
   * Classname of a Font Awesome icon: https://fontawesome.com/
   */
  faClass: string
}

/**
 * The schema of a VendorApp LocationInventory Model
 */
interface IVALocationInventory {
  /**
   * The unique id of the of the Location Inventory
   */
  id: number
  /**
   * The specified VendorApp location
   */
  location: IVALocation
  /**
   * The product that the location currently has in inventory
   */
  product: IVAProduct
  /**
   * The amount of the product the location currently hass
   */
  quanitity: number
}

/**
 * The schema for retrieving a list of all known locations and a specified target location's inventory
 */
interface IVAProductInfo {
  /**
   * All known VendorApp Locations
   */
  locations: IVALocation[]
  /**
   * The inventory for a specified
   */
  targetInventory: IVALocationInventory[]
  /**
   * List of all known catagories
   */
  catagories: IVACatagory[]
}

// * Loader component

/**
 * A centered react-spinner
 * @param props Includes size to indicate how large the loader should be
 */
const CenteredBeatLoader: React.FC<{ size: number }> = (props) => {
  return (
    <div
      style={{
        height: "100%",
        width: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <BeatLoader color={"#42c7b7"} size={props.size} />
    </div>
  )
}

const InventoryList: React.FC<{
  /**
   * List of inventory records
   */
  targetInventoryList: IVALocationInventory[]
  /**
   * input value from the user
   */
  inputValue: string
  /**
   * The id of the location of interest
   */
  currentLocationId: number
  /**
   * Function used to create a url
   */
  getProductUrl: (productId: number, locationId: number) => string
}> = (props) => {
  return (
    <ListGroup style={{ maxHeight: "600px", overflowY: "auto" }}>
      {props.targetInventoryList.length === 0 ? (
        <CenteredBeatLoader size={100} />
      ) : (
        props.targetInventoryList
          // Filter the list to whatever the user inputs in the textfield
          .filter(
            (tI) => new RegExp(props.inputValue, "i").test(tI.product.name) // case insensitive string comparison: https://stackoverflow.com/questions/44469548/es6-filter-data-with-case-insensitive-term
          )
          .map((inventoryItem, key) => (
            <ListGroup.Item key={key} className="inventory-item">
              <i
                style={{ fontSize: 50 }}
                className={`${inventoryItem.product.faClass}`}
              ></i>{" "}
              | Product: {inventoryItem.product.name} | In Stock:{" "}
              {inventoryItem.quanitity} | {/* Disable button if quantity is 0*/}
              {inventoryItem.quanitity > 0 ? (
                <Button
                  variant="primary"
                  // Have the button link to the details page of the product
                  href={props.getProductUrl(
                    inventoryItem.product.id,
                    props.currentLocationId
                  )}
                >
                  See Details
                </Button>
              ) : (
                <Button variant="outline-secondary" href="#" disabled>
                  OUT OF STOCK
                </Button>
              )}
            </ListGroup.Item>
          ))
      )}
    </ListGroup>
  )
}

// * Styling for ProductSearch Component

const AsideMenu = Styled.div`
height: 100%;
border-radius: 20px;
padding: 10px;
`

/**
 * List of available sorting methods
 */
enum SortOrder {
  Alphabetical,
  ReverseAlphabetical,
  HighPrice,
  LowPrice,
}

// TODO: add docs
interface IState {
  /**
   * The user input to filter products
   */
  inputValue: string
  /**
   * An array of locations retreived from the AJAX request
   */
  locations: IVALocation[]
  /**
   * An array of catagories retreived from the AJAX request
   */
  catagories: IVACatagory[]
  /**
   * A list of inventory records from a specific location
   */
  targetInventory: IVALocationInventory[]
  /**
   * The id of the location who's inventory is currently being shown
   */
  currentLocationId: number
  /**
   * The catagory that will be used to filter the list of products
   */
  currentCatagoryFilter?: string
  /**
   *
   */
  curretnSort: SortOrder
  /**
   * determines whether or not the component is still waiting on a response from the server
   */
  loading: boolean
}

// * The ProductSearch component

/**
 * A handy componnt for retreiving a list of products from the VenorApp DB, as well as listing all
 * the known locations.
 */
export default class ProductSearch extends React.Component<{}, IState> {
  /**
   * url definition can be found in Views\Product\Index.cshtml
   * relpace vaId with the prodcut's id and vaLocationId with the location's id
   */
  private readonly productDetailsUrl: string
  /**
   * This will store the entire list of products pull in from the AJAX request.
   *
   * To be used to restore the targetInventory state after being filtered
   */
  private savedProductList: IVALocationInventory[]

  constructor(props: React.Props<{}>) {
    super(props)

    this.productDetailsUrl = (window as any).productDetailsUrl

    this.state = {
      inputValue: "",
      locations: [],
      catagories: [],
      targetInventory: [],
      currentLocationId: 1,
      curretnSort: SortOrder.Alphabetical,
      loading: true,
    }

    // * binding
    this.handleProductInputChange = this.handleProductInputChange.bind(this)
    this.handleLocationClick = this.handleLocationClick.bind(this)
    this.handleSortClick = this.handleSortClick.bind(this)
    this.getShowProductUrl = this.getShowProductUrl.bind(this)
    this.filterListByCatagory = this.filterListByCatagory.bind(this)
    this.sortList = this.sortList.bind(this)
  }

  componentDidMount() {
    // signal that we're await for a response
    this.setState({ loading: true })
    setTimeout(() => {
      this.productInfoRequest()
    }, 1500)
  }

  componentDidUpdate(_prevProps: React.Props<{}>, prevState: IState) {
    if (this.state.currentLocationId !== prevState.currentLocationId) {
      // location id has been update, request data
      // signal that we're await for a response
      this.setState({ loading: true })
      this.productInfoRequest()
    }

    if (this.state.currentCatagoryFilter !== prevState.currentCatagoryFilter) {
      this.filterListByCatagory()
    }

    if (this.state.curretnSort !== prevState.curretnSort) {
      this.sortList(this.state.targetInventory)
    }
  }

  productInfoRequest() {
    axios
      .get(
        `https://localhost:5001/Product/GetProductInfo?locationId=${this.state.currentLocationId}`
      )
      .then((resp: AxiosResponse<IVAProductInfo>) => {
        // Sort and store retrieve inventory list and applay it to the state
        this.savedProductList = this.sortList(resp.data.targetInventory)

        this.setState({
          locations: resp.data.locations,
          targetInventory: this.savedProductList,
          catagories: resp.data.catagories,
        })
        this.filterListByCatagory() // TODO: handle how to filter/sort lists more effectively
        // this changes the state twice
      })
      // tslint:disable-next-line: no-console
      .catch((err) => console.log(err))
      .finally(() => this.setState({ loading: false }))
  }

  filterListByCatagory() {
    let tempList: IVALocationInventory[] = this.sortList(this.savedProductList)
    // if current catagory is set to null clear fliter
    if (!this.state.currentCatagoryFilter) {
      return
    }
    // set state as the filtered list
    tempList = tempList.filter(
      (tI) => tI.product.catagory === this.state.currentCatagoryFilter
    )
    this.setState({ targetInventory: tempList })
  }

  sortList(productList: IVALocationInventory[]): IVALocationInventory[] {
    // make default fitler alphabetical
    // switch case block for each filter (alphabetical, price)

    let tempList: IVALocationInventory[] = productList // failsafe, atleast return something

    const tempAlphabeticalList = productList.sort((a, b) => {
      if (a.product.name.toLowerCase() < b.product.name.toLowerCase()) return -1
      if (a.product.name.toLowerCase() > b.product.name.toLowerCase()) return 1
      return 0
    })

    switch (this.state.curretnSort) {
      case SortOrder.Alphabetical:
        tempList = tempAlphabeticalList
        break
      case SortOrder.ReverseAlphabetical:
        tempList = tempAlphabeticalList.reverse()
        break

      default:
        break
    }
    // set state after sorting
    this.setState({ targetInventory: tempList })

    return tempList
  }

  handleProductInputChange(e: React.ChangeEvent<HTMLInputElement>) {
    if (this.state.targetInventory.length === 0) {
      return
    }

    this.setState({ inputValue: e.target.value })
  }

  handleLocationClick(e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) {
    // Return if we're still waiting for a response from the server
    if (this.state.loading) {
      return
    }

    // get list item's data-location-id
    const locationId = e.currentTarget
      .querySelector("[data-location-id]")
      ?.getAttribute("data-location-id")

    // update current location id state and request inventory data from server
    this.setState({ currentLocationId: Number(locationId) }) // TODO: handle case where locationId may not be a number
  }

  handleCatagoryClick(
    e: React.MouseEvent<HTMLAnchorElement, MouseEvent>,
    catagoryFilter: string
  ) {
    // check if clicked catagory was already selected
    // if it was, clear filter and return
    if (
      this.state.currentCatagoryFilter &&
      this.state.currentCatagoryFilter === catagoryFilter
    ) {
      this.setState({ currentCatagoryFilter: undefined })
      return
    }

    // set current catagory to trigger the filter
    this.setState({ currentCatagoryFilter: catagoryFilter })
  }

  handleSortClick(
    e: React.MouseEvent<HTMLElement, MouseEvent>,
    sort: SortOrder
  ) {
    this.setState({ curretnSort: sort })
  }

  /**
   * Create's a url that will direct to a Product's details page with a
   * specified location
   * @param urlTemplate The template
   * @param productId The product's id
   * @param locationId The id of the location
   */
  getShowProductUrl(productId: number, locationId: number): string {
    // use the url template that was created from the view
    return this.productDetailsUrl
      .replace("vaId", `${productId}`)
      .replace("vaLocationId", `${locationId}`)
  }

  render() {
    // * List of all known locations
    const locationsList =
      this.state.locations?.length === 0 ? (
        <CenteredBeatLoader size={25} />
      ) : (
        this.state.locations?.map((location, key) => (
          <ListGroup.Item
            key={key}
            active={location.id === this.state.currentLocationId}
            onClick={this.handleLocationClick}
          >
            <div data-location-id={`${location.id}`}>{location.name}</div>
          </ListGroup.Item>
        ))
      )

    // * List of products to display to the user
    const inventoryList =
      this.state.targetInventory.length === 0 ? (
        <CenteredBeatLoader size={50} />
      ) : (
        <InventoryList
          targetInventoryList={this.state.targetInventory}
          inputValue={this.state.inputValue}
          currentLocationId={this.state.currentLocationId}
          getProductUrl={this.getShowProductUrl}
        />
      )

    // * create list of catagories for the aside
    const catagoryList =
      this.state.catagories?.length === 0 ? (
        <CenteredBeatLoader size={25} />
      ) : (
        this.state.catagories?.map((catagory, key) => (
          <ListGroup.Item
            key={key}
            active={catagory.catagoryName === this.state.currentCatagoryFilter}
            onClick={(e: any) =>
              this.handleCatagoryClick(e, catagory.catagoryName)
            }
          >
            <div data-location-id={`${catagory.id}`}>
              {catagory.catagoryName}
            </div>
          </ListGroup.Item>
        ))
      )

    // * create list of sorting options
    const sortList =
      this.state.catagories?.length === 0 ? (
        <CenteredBeatLoader size={25} />
      ) : (
        <ListGroup>
          <ListGroup.Item
            /* Apply onClick events to sort product list */
            onClick={(e: any) =>
              this.handleSortClick(e, SortOrder.Alphabetical)
            }
            // Set as an active bootstrap list-item
            active={this.state.curretnSort === SortOrder.Alphabetical}
          >
            Alphabetical
          </ListGroup.Item>
          <ListGroup.Item
            onClick={(e: any) =>
              this.handleSortClick(e, SortOrder.ReverseAlphabetical)
            }
            active={this.state.curretnSort === SortOrder.ReverseAlphabetical}
          >
            Reverse Alphabetical
          </ListGroup.Item>
        </ListGroup>
      )

    return (
      <Container
        style={{
          minHeight: 700,
          borderRadius: 20,
          padding: 20,
        }}
      >
        <Row style={{ height: "100%" }}>
          <Col xs={3} style={{ height: "100%" }}>
            <AsideMenu>
              {/* Location's List */}
              <h3 className="location-aside-menu-title">Locations</h3>
              <hr />
              <ListGroup>{locationsList}</ListGroup>
              {/* Catagory's List */}
              <br />
              <h3 className="product-aside-menu-title">Catagories</h3>
              <hr />
              {catagoryList}
              {/* Sorting List */}
              <br />
              <h3 className="sorting-aside-menu-title">Sorting</h3>
              <hr />
              {sortList}
            </AsideMenu>
          </Col>
          <Col xs={9} style={{ height: "100%" }}>
            <FormControl
              type="text"
              placeholder="Search For A Product"
              onChange={this.handleProductInputChange}
            />
            <div style={{ height: "100%", marginTop: 10 }}>{inventoryList}</div>
          </Col>
        </Row>
      </Container>
    )
  }
}
