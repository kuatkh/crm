/* global __dirname */
const webpack = require('webpack')
var path = require('path')
var FileSystem = require('fs')
const CompressionPlugin = require('compression-webpack-plugin')
const TerserPlugin = require("terser-webpack-plugin")
const HtmlWebpackPlugin = require('html-webpack-plugin')
const FaviconsWebpackPlugin = require('favicons-webpack-plugin')

module.exports = {
	entry: [
		'whatwg-fetch',
		'react-hot-loader/patch',
		'./src/index.js',
	],
	optimization: {
		minimize: true,
		minimizer: [
			new TerserPlugin({
				parallel: true,
			}),
		],
	},
	output: {
		publicPath: './',
		path: path.resolve(__dirname, 'build'),
		filename: 'bundle-[fullhash].js',
		clean: true,
	},
	devServer: {
		historyApiFallback: true,
	},
	plugins: [
		new webpack.optimize.AggressiveMergingPlugin(),
		new CompressionPlugin({
			filename: '[path][base].gz',
			algorithm: 'gzip',
			test: /\.js$|\.css$|\.html$/,
			threshold: 10240,
			minRatio: 0.8,
		}),
		new HtmlWebpackPlugin({
			title: 'CRM. Страница администратора',
			filename: 'index.html',
			template: 'index.html',
			xhtml: true,
		}),

		// https://github.com/itgalaxy/favicons#usage
		new FaviconsWebpackPlugin({
			logo: './src/art-fraffrog-moy-sosed-totoro.jpg',
			prefix: 'icons-[fullhash]/',
			emitStats: false,
			statsFilename: 'iconstats.json',
			persistentCache: false,
			inject: true,
			background: '#fff',
			title: 'CRM. Страница администратора',
			developerName: 'Kuat Khamitov',
			icons: {
				android: true,
				appleIcon: true,
				appleStartup: true,
				coast: false,
				favicons: true,
				firefox: true,
				opengraph: false,
				twitter: false,
				yandex: false,
				windows: true,
			},
		}),
		new webpack.DefinePlugin({
			'process.env': {
				NODE_ENV: JSON.stringify('production'),
			},
		}),
	],
	module: {
		rules: [
			{
				test: /\.(js|jsx)$/,
				exclude: /node_modules/,
				use: ['babel-loader'],
			},
			{
				test: /\.css$/,
				use: ['style-loader', 'css-loader'],
			},
			{
				test: /\.woff($|\?)|\.woff2($|\?)|\.ttf($|\?)|\.eot($|\?)|\.svg($|\?)/,
				use: ['url-loader?limit=20000&name=[name]-[fullhash].[ext]'],
			},
			{
				test: /\.(png|jpg|gif)$/,
				use: ['file-loader'],
			},
			{
				test: /\.?js$/,
				exclude: /(node_modules|bower_components)/,
				use: {
					loader: 'babel-loader',
					options: {
						presets: ['@babel/preset-react', '@babel/preset-env']
					}
				}
			},
			{
				test: /\.html$/,
				use: ['html-loader'],
			},
		],
	},
	resolve: {
		modules: [path.resolve(__dirname, 'src'), 'node_modules'],
		alias: {
			common: path.resolve(__dirname, 'src'),
		},
		fallback: {
		  'fs': false,
		  'tls': false,
		  'net': false,
		  'path': false,
		  'zlib': false,
		  'http': false,
		  'https': false,
		  'stream': false,
		  'crypto': false,
		  'events': false, // require.resolve('events/'); npm i events
		  'url': false, // require.resolve('url/'); npm i url
		  'vm': false, // require.resolve("vm-browserify"); npm i vm-browserify
		  'tty': false, // require.resolve("tty-browserify"); npm i tty-browserify
		  'console': require.resolve('console-browserify'),
		  'constants': require.resolve('constants-browserify'),
		  'assert': false, // require.resolve("assert/"); npm i assert
		  'querystring': false, // require.resolve("querystring-es3"); npm i querystring-es3
		  'os': require.resolve('os-browserify/browser'), // require.resolve("os-browserify/browser"); npm i os-browserify
		  'crypto-browserify': require.resolve('crypto-browserify'), // require.resolve('crypto-browserify'); npm i rypto-browserify
		},
	},
}
