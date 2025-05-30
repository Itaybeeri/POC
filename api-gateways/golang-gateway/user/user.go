// user.go
package main

import (
	"net/http"

	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
)

func main() {
	e := echo.New()

	e.Use(middleware.Logger())

	e.GET("/ping", func(c echo.Context) error {
		return c.String(http.StatusOK, "pong")
	})

	e.POST("/api/user/login", func(c echo.Context) error {
		return c.JSON(http.StatusOK, map[string]string{
			"message": "login successful",
			"token":   "mock-jwt-token",
		})
	})

	e.GET("/api/user/profile", func(c echo.Context) error {
		return c.JSON(http.StatusOK, map[string]interface{}{
			"id":    1,
			"name":  "John Doe",
			"email": "john@example.com",
		})
	})

	e.Logger.Fatal(e.Start(":5202"))
}
