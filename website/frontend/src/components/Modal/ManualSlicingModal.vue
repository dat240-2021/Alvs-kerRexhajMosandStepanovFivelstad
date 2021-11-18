<template>
  <div
    class="modal d-block"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-xl" id="slicingModal">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Manual Slicing</h5>
          <h5 class="modal-title" id="exampleModalLabel"></h5>
          <button type="button" class="btn-close" @click="closeModal"></button>
        </div>
        <div class="modal-body" id="slicingModalBody">
          <div class="position-relative">
            <img
              :src="modalImage.file"
              style="width: 100%; z-index: 1"
              id="original_image"
              zindex-dropdown
              :onload="resize"
            />
            <canvas
              id="canvas_modal"
              class="position-absolute top-0 start-0 opacity-75"
              style="z-index: 2"
            ></canvas>
          </div>
          <label for="customRange1" class="form-label">Brush Diameter: {{ lineWidth }}</label>
          <input
            type="range"
            v-model="lineWidth"
            class="form-range"
            min="0"
            max="80"
            step="1"
            id="customRange1"
          />
          <div class="d-flex justify-content-start h-100">
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

            <div
              class="mx-5 bg col-md-3 col-sm-3 col-xs-3"
              :style="'background-color:' + pickedColor"
            ></div>

            <button
              class="btn btn-primary"
              @click="selectAnotherColor = true"
              v-if="colorPicker == true"
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
      path: null as any,
      slice: null as any,
      coord: { x: 0, y: 0 },
      leavingDisabled: false,
      lineWidth: 5,
      pathColors: ["#171717", "#edf0ee"],
      colorToggler: false,
      sliceColors: [] as string[],
      colorPicker: false,
      selectAnotherColor: false,
      pickedColor: "",
    };
  },
  props: {
    modalImage: {
      type: Object as PropType<ImageFile>,
      required: true,
      default: {
        id: 0,
        name: "",
        file: null,
        category: "",
        label: "",
      } as ImageFile,
    },
  },
  mounted: function () {
    this.canvas = document.getElementById("canvas_modal");
    this.ctx = this.canvas.getContext("2d");
    document.addEventListener("mousedown", this.start);
    document.addEventListener("mouseup", this.stop);
    window.addEventListener("resize", this.resize);
  },
  methods: {
    resize() {
      var oImage = document.getElementById("original_image");
      this.ctx.canvas.width = oImage?.clientWidth;
      this.ctx.canvas.height = oImage?.clientHeight;
    },
    reposition(event: any) {
      var rel = document.getElementById("slicingModal");
      var rel2 = document.getElementById("slicingModalBody");
      if (rel == null || rel2 == null) {
        return;
      }
      this.coord.x = event.clientX - (rel.offsetLeft + rel2.offsetLeft + 16);
      this.coord.y = event.clientY - (rel.offsetTop + rel2.offsetTop + 16);
    },
    start(event: any) {
      var color = "";
      if (
        (this.colorPicker == true && this.pickedColor == "") ||
        this.selectAnotherColor
      ) {
        this.reposition(event);
        var pixel = this.ctx.getImageData(
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
      document.addEventListener("mousemove", this.draw);
      // this.slice = new Path2D();
      // this.reposition(event);
      this.ctx.strokeStyle = color;
      // this.slice.moveTo(this.coord.x, this.coord.y);
      this.reposition(event);
    },
    stop() {
      // this.ctx.closePath(this.slice);

      // this.ctx.fillStyle = this.newColor();
      // this.ctx.fill(this.slice);
      document.removeEventListener("mousemove", this.draw);
    },
    draw(event: any) {
      // this.colorToggler = this.colorToggler != true;
      this.ctx.beginPath();
      this.ctx.lineWidth = this.lineWidth;
      this.ctx.lineCap = "round";

      // this.ctx.strokeStyle = this.pathColors[this.colorToggler ? 1 : 0];
      this.ctx.moveTo(this.coord.x, this.coord.y);
      this.reposition(event);
      // this.slice.lineTo(this.coord.x, this.coord.y);
      this.ctx.lineTo(this.coord.x, this.coord.y);
      this.ctx.stroke();
    },

    newColor() {
      const randomColor = () => {
        var letters = '0123456789ABCDEF';
        var color = '#';
        for (var i = 0; i < 6; i++) {
          color += letters[Math.floor(Math.random() * 16)];
        }
        console.log(color);
        return color;
      };

      var color = randomColor();
      // if we find the same color try a new one.
      while (this.sliceColors.find((x) => x == color) != null) {
      color = randomColor();
      }
      this.sliceColors.push(color);
      return color;
    },

    saveAndExit() {
      var data = this.canvas.toDataURL("image/png", 1.0);

      this.$emit("SaveAndExit", { id: this.modalImage.id, data: data,colors: this.sliceColors });
    },
    closeModal() {
      this.$emit("closeModal");
    },
    rgbToHex(r: number, g: number, b: number) {
      const componentToHex = (c: number) => {
        var hex = c.toString(16);
        return hex.length == 1 ? "0" + hex : hex;
      };
      return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
    },
  },
});
</script>