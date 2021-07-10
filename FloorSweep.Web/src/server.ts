import sirv from "sirv";
import express from "express";
import compression from "compression";
import * as sapper from "@sapper/server";

const { PORT, NODE_ENV, HOSTNAME } = process.env;
const dev = NODE_ENV === "development";
console.log(HOSTNAME);
express() // You can also use Express
  .use(
    compression({ threshold: 0 }),
    sirv("static", { dev }),
    sirv("src", { dev }),
    sapper.middleware()
  )
  .listen(parseInt(PORT), HOSTNAME);
