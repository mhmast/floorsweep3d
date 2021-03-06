export function getClientCoordinates(e: MouseEvent): Point {
  return { x: e.offsetX, y: e.offsetY };
}

export interface Point {
  x: number;
  y: number;
}

export interface Line {
  start: Point;
  end: Point;
}

export interface Color {
  r: number;
  g: number;
  b: number;
  a: number;
}

export const index = (l: Point, width: number) => (l.x + l.y * width) * 4;
export const getPixel = (l: Point, data: ImageData): Color => {
  return {
    r: data.data[index(l, data.width)],
    g: data.data[index(l, data.width) + 1],
    b: data.data[index(l, data.width) + 2],
    a: data.data[index(l, data.width) + 3],
  };
};
export const setPixel = (l: Point, data: ImageData, color: Color) => {
  data.data[index(l, data.width)] = color.r;
  data.data[index(l, data.width) + 1] = color.g;
  data.data[index(l, data.width) + 2] = color.b;
  data.data[index(l, data.width) + 3] = color.a;
};

export const d2r = (d: number): number => (Math.PI / 180) * d;

export function drawLine(context: CanvasRenderingContext2D, line: Line) {
  context.save();
  context.beginPath();
  context.strokeStyle = "red";
  context.lineWidth = 2;
  context.moveTo(line.start.x, line.start.y);
  context.lineTo(line.end.x, line.end.y);
  context.stroke();
  context.restore();
}

export function drawImage(
  context: CanvasRenderingContext2D,
  image: CanvasImageSource,
  x: number,
  y: number,
  rotation?: number,
  scalex?: number,
  scaley?: number,
  center?: boolean
): ImageData {
  if (!scalex) scalex = 1;
  if (!scaley) scaley = 1;
  context.save();

  const w = image.width as number;
  const h = image.height as number;
  const halfw = (w * scalex) / 2;
  const halfh = (h * scalex) / 2;

  context.translate(x, y);
  if (rotation) {
    context.rotate(d2r(rotation));
  }
  context.translate(-x, -y);
  if (center) {
    context.drawImage(image, x - halfw, y - halfh, w * scalex, h * scaley);
  } else {
    context.drawImage(image, x, y, w * scalex, h * scaley);
  }
  const imageDta = context.getImageData(0, 0, w, h);

  context.restore();
  return imageDta;
}

export const rotatePoint = (l: Point, rot: number): Point => {
  const rad = d2r(rot);
  const x = Math.round(l.x * Math.cos(rad) - l.y * Math.sin(rad));
  const y = Math.round(l.x * Math.sin(rad) + l.y * Math.cos(rad));
  return { x, y };
};

export const addPoint = (p1: Point, p2: Point): Point => {
  return { x: p1.x + p2.x, y: p1.y + p2.y };
};

export const convertBlackAndWhite = (imgdta: ImageData): ImageData => {
  for (let y = 0; y < imgdta.height; y++) {
    for (let x = 0; x < imgdta.width; x++) {
      const color = getPixel({ x, y }, imgdta);
      const rgb = color.r + color.g + color.b;
      if (rgb <= (255 * 3) / 2) {
        setPixel({ x, y }, imgdta, { r: 0, g: 0, b: 0, a: color.a });
      } else {
        setPixel({ x, y }, imgdta, { r: 255, g: 255, b: 255, a: color.a });
      }
    }
  }

  return imgdta;
};
