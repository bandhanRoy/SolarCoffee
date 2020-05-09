<template>
  <div class="inventory-container">
    <h1 id="inventory-title">Inventory Dashboard</h1>
    <hr />
    <div class="inventory-actions">
      <solar-button @button:click="showNewProductModal" id="addNewBtn">Add new item</solar-button>
      <solar-button @button:click="showShipMentModal" id="recieveShipmentBtn">Recieve Shipment</solar-button>
    </div>
    <table id="inventory-table" class="table">
      <tr>
        <th>Item</th>
        <th>Quantity On-Hand</th>
        <th>Unit Price</th>
        <th>Taxable</th>
        <th>Delete</th>
      </tr>
      <tr v-for="item in inventory" :key="item.id">
        <td>{{item.product.name}}</td>
        <td
          v-bind:class="`${applyColor(item.quantityOnHand, item.idealQuantity)}`"
        >{{item.quantityOnHand}}</td>
        <td>{{item.product.price | price}}</td>
        <td>
          <span v-if="item.product.isTaxable">Yes</span>
          <span v-else>No</span>
        </td>
        <td>
          <div
            class="lni lni-cross-circle product-archive"
            @click="archiveProduct(item.product.id)"
          ></div>
        </td>
      </tr>
    </table>

    <product-modal v-if="isNewProductVisible" @save:product="savenewProduct" @close="closeModals" />
    <shipment-modal
      v-if="isShipmentVisible"
      :inventory="inventory"
      @save:shipment="saveNewShipment"
      @close="closeModals"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IProductInventory, IProduct } from "@/types/Product.d.ts";
import SolarButton from "@/components/SolarButton.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import ProductModal from "@/components/modals/ProductModal.vue";
import { IShipment } from "../types/Shipment";
import { InventoryService } from "@/services/inventory-service.ts";
import { ProductService } from "@/services/product-service.ts";

@Component({
  name: "Inventory",
  components: { SolarButton, ShipmentModal, ProductModal }
})
export default class Inventory extends Vue {
  inventoryService = new InventoryService();
  productService = new ProductService();
  inventory: IProductInventory[] = [];
  isNewProductVisible = false;
  isShipmentVisible = false;

  closeModals() {
    this.isShipmentVisible = false;
    this.isNewProductVisible = false;
  }

  showNewProductModal() {
    this.isNewProductVisible = true;
  }

  showShipMentModal() {
    this.isShipmentVisible = true;
  }

  async savenewProduct(newProduct: IProduct) {
    try {
      await this.productService.addNewProduct(newProduct);
      await this.initialize();
      this.isNewProductVisible = false;
    } catch (error) {
      console.error("Error Occurred while adding new product", error);
    }
  }
  async saveNewShipment(shipment: IShipment) {
    try {
      await this.inventoryService.updateInventoryQuantity(shipment);
      await this.initialize();
      this.isShipmentVisible = false;
    } catch (error) {
      console.error("Error Occurred while updating inventory qunatity", error);
    }
  }

  async initialize() {
    this.inventory = await this.inventoryService.getInventory();
  }

  async created() {
    await this.initialize();
  }

  applyColor(currentValue: number, targetValue: number) {
    if (currentValue <= 0) {
      return "red";
    }
    if (Math.abs(targetValue - currentValue) > 8) {
      return "yellow";
    }
    return "green";
  }

  async archiveProduct(productId: number) {
    try {
      await this.productService.archive(productId);
      await this.initialize();
    } catch (error) {
      console.error("Error Occured");
    }
  }
}
</script>

<style lang="scss">
@import "@/scss/global.scss";
.green {
  font-weight: bold;
  color: $solar-green;
}

.yellow {
  font-weight: bold;
  color: $solar-yellow;
}

.red {
  font-weight: bold;
  color: $solar-red;
}

.inventory-actions {
  display: flex;
  justify-content: flex-end;
  margin-bottom: 0.8rem;
}

.product-archive {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $solar-red;
}
</style>
