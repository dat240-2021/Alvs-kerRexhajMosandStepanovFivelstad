<template>
  <div
    class="modal d-block"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
    id="manSlicingModal"
  >
    <div class="modal-dialog" id="slicingModal" style="display: table;">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title text-center w-90" id="exampleModalLabel">
            Manual Slicing
          </h5>
          <h5 class="modal-title" id="exampleModalLabel"></h5>
          <button type="button" class="btn-close" @click="closeModal"></button>
        </div>
        <div class="modal-body p-1" id="slicingModalBody">
          <div class="position-relative w-100 h-100">
            <img
              :src="modalImage.file"
              id="original_image"
              zindex-dropdown
              :onload="imageLoaded"
              style="width:100%"
            />
            <canvas
              id="canvas_modal"
              class="position-absolute top-0 start-0 opacity-75"
              style="z-index: 2"
            ></canvas>
          </div>
          <label for="customRange1" class="form-label mt-1"
            >Brush Diameter: {{ lineWidth }}</label
          >
          <input
            type="range"
            v-model="lineWidth"
            class="form-range my-3"
            min="0"
            max="80"
            step="1"
            id="customRange1"
          />
          <div class="d-flex justify-content-between h-100">

            <button
              type="button"
              class="btn btn-outline-primary"
              @click="
                colorPicker = colorPicker != true;
                pickedColor = '';
              "
              data-bs-toggle="button"
            >
              Color Picker
            </button>
            <div v-if="!colorPicker">
              <p class="text-center">Color Picker Disabled</p>
            </div>

            <div
              class="col-2 col-md-3 col-sm-3 mx-1"
              :style="'background-color:' + pickedColor"
            ></div>

            <button
              class="btn btn-primary  mx-1"
              @click="selectAnotherColor = true"
              v-if="colorPicker"
            >
              Pick Again
            </button>

            <button class="btn btn-primary" @click="saveAndExit">
              Finished
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import { ImageFile } from "@/typings";

export default defineComponent({
  name: "ManualSlicingModal",
  data() {
    return {
      canvas: null as any,
      ctx: null as any,
      slice: null as any,
      coord: { x: 0, y: 0 },
      leavingDisabled: false,
      lineWidth: 5,
      colorToggler: false,
      sliceColors: [] as string[],
      colorPicker: false,
      selectAnotherColor: false,
      pickedColor: "",
      currentColor: "",
      modal: null as any,
    };
  },
  watch: {
    currentColor: function (newColor: string) {
      if (!this.sliceColors.includes(newColor)) {
        this.sliceColors = [...this.sliceColors, newColor];
      }
    },
  },
  props: {
    modalImage: {
      type: Object as PropType<ImageFile>,
      required: true,
      default: {
        id: 0,
        name: "",
        file: null,
        sliceFile: "",
        category: "",
        label: "",
      } as ImageFile,
    },
  },
  mounted: function () {
    this.canvas = document.getElementById("canvas_modal");
    this.modal = document.getElementById("manSlicingModal");
    this.ctx = this.canvas.getContext("2d");
    this.canvas.addEventListener("mousedown", this.start);
    this.canvas.addEventListener("mouseup", this.stop);
    window.addEventListener("resize", this.resize);

    // Set up touch events for mobile, etc
    this.canvas.addEventListener(
      "touchstart",
      function (e: any) {
        let canvas = document.getElementById("canvas_modal");
        if (canvas == null) {
          return;
        }
        let mousePos = getTouchPos(canvas, e);
        var touch = e.touches[0];
        var mouseEvent = new MouseEvent("mousedown", {
          clientX: touch.clientX,
          clientY: touch.clientY,
        });

        canvas.dispatchEvent(mouseEvent);
      },
      false
    );

    this.canvas.addEventListener(
      "touchend",
      function (e: any) {
        var mouseEvent = new MouseEvent("mouseup", {});
        document.getElementById("canvas_modal")?.dispatchEvent(mouseEvent);
      },
      false
    );

    this.canvas.addEventListener(
      "touchmove",
      function (e: any) {
        var touch = e.touches[0];
        var mouseEvent = new MouseEvent("mousemove", {
          clientX: touch.clientX,
          clientY: touch.clientY,
        });
        document.getElementById("canvas_modal")?.dispatchEvent(mouseEvent);
      },
      false
    );

    // Get the position of a touch relative to the canvas
    function getTouchPos(canvasDom: any, touchEvent: any) {
      var rect = canvasDom.getBoundingClientRect();
      return {
        x: touchEvent.touches[0].clientX - rect.left,
        y: touchEvent.touches[0].clientY - rect.top,
      };
    }
  },
  methods: {
    imageLoaded(event: any) {
      // // document.getElementById("canvas_modal").style();
      // if (event.target.width > event.target.height) {
      //   // if the image is wider than its tall,
      //   // 80 vh should ensure it won't exceed window height
      //   event.target.style = "width:80vh;";
      // } else {
      //   event.target.style = "height:50vh;";
      // }
      if (this.modalImage.sliceFile != "") {
        this.reloadData(this.modalImage.sliceFile);
      }
      this.resize();
    },
    reloadData(data: string) {
      let image = new Image();
      image.src = data;
      image.onload = () => {
        this.ctx.drawImage(image, 0, 0, this.canvas.width, this.canvas.height);
      };
    },
    resize() {
      let data = this.canvas.toDataURL("image/png", 1.0);
      let oImage = document.getElementById("original_image");
      this.ctx.canvas.width = oImage?.clientWidth;
      this.ctx.canvas.height = oImage?.clientHeight;
      this.reloadData(data);
    },
    reposition(event: any) {
      let rel = document.getElementById("slicingModal");
      let rel2 = document.getElementById("slicingModalBody");
      if (rel == null || rel2 == null) {
        return;
      }
      this.coord.x = event.clientX - (rel.offsetLeft + rel2.offsetLeft + 4);
      this.coord.y = event.clientY - (rel.offsetTop + rel2.offsetTop + 4);
    },

    start(event: any) {
      let color = "";
      if (
        (this.colorPicker && this.pickedColor == "") ||
        this.selectAnotherColor
      ) {
        this.reposition(event);
        let pixel = this.ctx.getImageData(
          this.coord.x,
          this.coord.y,
          this.canvas.width,
          this.canvas.height
        ).data;
        this.pickedColor = this.rgbToHex(pixel[0], pixel[1], pixel[2]);
        this.selectAnotherColor = false;
      }

      if (this.pickedColor == "") {
        color = this.newColor();
      } else {
        color = this.pickedColor;
      }
      this.canvas.addEventListener("mousemove", this.draw);
      this.currentColor = color;
      this.ctx.strokeStyle = color;
      this.reposition(event);
    },
    stop() {
      this.canvas.removeEventListener("mousemove", this.draw);
    },
    draw(event: any) {
      this.ctx.beginPath();
      this.ctx.lineWidth = this.lineWidth;
      this.ctx.lineCap = "round";

      this.ctx.moveTo(this.coord.x, this.coord.y);
      this.reposition(event);
      this.ctx.lineTo(this.coord.x, this.coord.y);
      this.ctx.stroke();
    },

    newColor() {
      const randomColor = () => {
        let letters = "0123456789ABCDEF";
        let color = "#";
        for (let i = 0; i < 6; i++) {
          color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
      };

      let color = randomColor();
      // if we find the same color try a new one.
      while (this.sliceColors.find((x) => x == color) != null) {
        color = randomColor();
      }
      return color;
    },

    saveAndExit() {
      this.clearEventListeners();

      let data = this.canvas.toDataURL("image/png", 1.0);

      this.$emit("SaveAndExit", {
        id: this.modalImage.id,
        data: data,
        colors: this.sliceColors,
      });
    },
    closeModal() {
      this.clearEventListeners();
      this.$emit("closeModal");
    },
    clearEventListeners() {
      this.canvas.removeEventListener("mousedown", this.start);
      this.canvas.removeEventListener("mouseup", this.stop);
      window.removeEventListener("resize", this.resize);
      this.canvas.removeEventListener("mousemove", this.draw);
    },
    rgbToHex(r: number, g: number, b: number) {
      const componentToHex = (c: number) => {
        let hex = c.toString(16);
        return hex.length == 1 ? "0" + hex : hex;
      };
      return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
    },
  },
});
</script>
