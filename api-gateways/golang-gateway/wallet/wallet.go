// wallet.go
package main

import (
	"net/http"

	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
)

func main() {
	e := echo.New()

	e.Use(middleware.Logger())

	e.GET("/api/wallet/balance", func(c echo.Context) error {
		return c.JSON(http.StatusOK, map[string]interface{}{
			"balance":  150.75,
			"currency": "USD",
		})
	})

	e.POST("/api/wallet/charge", func(c echo.Context) error {
		return c.JSON(http.StatusOK, map[string]string{
			"status": "charged",
		})
	})

	e.Logger.Fatal(e.Start(":5201"))
}
